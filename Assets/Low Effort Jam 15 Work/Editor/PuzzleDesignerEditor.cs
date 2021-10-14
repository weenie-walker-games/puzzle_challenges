using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LowEffort
{
    public class PuzzleDesignerEditor : EditorWindow
    {
        //Store the puzzle dimensions
        int puzzleWidth;
        int puzzleHeight;
        Vector2 currentPuzzleDimensions;

        //Window and puzzle view variables
        static Vector2 windowSize = new Vector2(1200, 950);
        Rect puzzleShapeRect = new Rect(350, 100, 800, 800);
        bool isPuzzleActive;

        //Border and piece options
        GUIStyle pieceStyle = new GUIStyle();
        RectOffset border;
        static Texture2D boxBorder;
        static GUIStyle boxStyle = new GUIStyle();
        static string borderName = "OuterBorder_white.png";

        //Store the values of the puzzle pieces and the 
        BackingValue[,] resultsArray = new BackingValue[40, 40];
        static List<Color> colors = new List<Color>();
        static int numOfColors = 3;
        Color currentActiveColor = Color.black;
        


        [MenuItem("Tools/Puzzle Creator")]
        public static void Init()
        {
            PuzzleDesignerEditor designer = (PuzzleDesignerEditor)EditorWindow.GetWindow(typeof(PuzzleDesignerEditor));
            designer.minSize = windowSize;
            designer.maxSize = windowSize;
            designer.titleContent.text = "Puzzle Creator";
            designer.Show();


            //Find the border for the boxes
            boxBorder = EditorGUIUtility.FindTexture("Assets/Shared Assets/Textures/" + borderName);
            if (boxBorder == null)
            {
                Debug.LogError("The border texture was not found");
            }

            boxStyle.normal.background = boxBorder;

            ResetColors();
        }

        private void OnGUI()
        {
            Event e = Event.current;

            DrawHeader(e);

            DrawSideButtons(e);


            //Draw the board
            if (isPuzzleActive)
            {
                GUILayout.BeginArea(puzzleShapeRect);
                DrawBoard(e);
                GUILayout.EndArea();
            }

            //Right click
            if(e.type == EventType.MouseDown && Event.current.button == 1)
            {
                //Reset the color
            }

            //Left mouse
            if(e.type == EventType.MouseDown && Event.current.button == 0)
            {

            }
        }

        private void DrawHeader(Event e)
        {
            GUILayout.BeginArea(new Rect(0, 0, 600, 150));
            GUILayout.Label("Puzzle Settings", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            puzzleHeight = (int)EditorGUILayout.Slider("Puzzle Height", puzzleHeight, 8, 40);
            puzzleWidth = (int)EditorGUILayout.Slider("Puzzle Width", puzzleWidth, 8, 40);
            EditorGUILayout.EndHorizontal();
            numOfColors = (int)EditorGUILayout.Slider("Number of Colors", numOfColors, 3, 9);
            EditorGUILayout.EndVertical();


            //Draw button to build or reset the puzzle
            GUILayout.BeginHorizontal();
            string buttonName = isPuzzleActive ? "Reset Puzzle" : "Build Blank Puzzle";
            if (GUILayout.Button(buttonName))
            {
                BuildBlankPuzzle();
            }
            if (GUILayout.Button("Save File"))
            {
                //NEED TO BUILD THIS FUNCTION
                SaveFile();
            }

            if (GUILayout.Button("Load File"))
            {
                //NEED TO BUILD THIS FUNCTION
                LoadFile();
            }

            GUILayout.EndHorizontal();

            GUILayout.EndArea();
        }

        private void DrawSideButtons(Event e)
        {
            GUILayout.BeginArea(new Rect(50, 150, 200, 750));

            Rect colorSwatch = new Rect(0, 0, 75, 75);

            GUILayout.BeginVertical();
            GUI.backgroundColor = Color.black;

            for (int i = 0; i < numOfColors; i++)
            {
                colorSwatch.y = i * 85;

                EditorGUI.DrawRect(colorSwatch, colors[i]);
                //GUI.Box(colorSwatch, "", boxStyle);
                if(GUI.Button(colorSwatch, "", boxStyle))
                {
                    currentActiveColor = colors[i];
                }

                GUILayout.Space(10);
            }

            GUI.backgroundColor = Color.white;

            GUILayout.EndVertical();
            GUILayout.EndArea();
        }

        private void BuildBlankPuzzle()
        {
            isPuzzleActive = true;

            //reset all info; amounts are hard coded in
            resultsArray.Initialize();
            for (int i = 0; i < 40; i++)
            {
                for (int j = 0; j < 40; j++)
                {
                    resultsArray[i, j] = new BackingValue(new Vector2(i, j), Color.clear);
                }
            }

            currentPuzzleDimensions.x = puzzleHeight;
            currentPuzzleDimensions.y = puzzleWidth;
        }

        private void DrawBoard(Event e)
        {

            Rect basePiece = new Rect();
            float size = DeterminePieceSize();
            basePiece.height = size;
            basePiece.width = size;
            border = GUI.skin.button.border;
            for (int i = 0; i < currentPuzzleDimensions.x; i++)
            {
                for (int j = 0; j < currentPuzzleDimensions.y; j++)
                {
                    basePiece.x = resultsArray[i, j].location.x * size; //basePiece.width;
                    basePiece.y = resultsArray[i, j].location.y * size; //basePiece.height;
                    EditorGUI.DrawRect(new Rect(basePiece), resultsArray[i,j].color);

                    //Draw the border texture and change the texture color
                    GUI.backgroundColor = Color.black;
                    if(GUI.Button(basePiece, "", boxStyle))
                    {
                        resultsArray[i, j].color = currentActiveColor;
                    }
                    GUI.backgroundColor = Color.white;
                }
            }
        }

        /// <summary>
        /// Make each piece fit the puzzle area and be squares based on the smallest dimension side
        /// </summary>
        /// <returns></returns>
        private float DeterminePieceSize()
        {
            float height = puzzleShapeRect.height / currentPuzzleDimensions.x;
            float width = puzzleShapeRect.width / currentPuzzleDimensions.y;

            return Mathf.Min(height, width);
        }

        private void SaveFile()
        {

        }

        private void LoadFile()
        {

        }

        private static void ResetColors()
        {
            colors.Clear();
            colors.Add(Color.clear);
            colors.Add(Color.black);
            colors.Add(Color.white);
            colors.Add(Color.blue);
            colors.Add(Color.green);
            colors.Add(Color.yellow);
            colors.Add(Color.red);
            colors.Add(Color.cyan);
            colors.Add(Color.magenta);
        }

    }

    [System.Serializable]
    public class BackingValue
    {
        public Vector2 location = Vector2.zero;
        public Color color = Color.green;

        public BackingValue()
        {

        }

        public BackingValue(Vector2 location, Color color)
        {
            this.location = location;
            this.color = color;
        }


    }
}
