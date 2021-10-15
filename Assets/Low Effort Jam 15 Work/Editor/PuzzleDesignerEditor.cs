using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Puzzle;
using System.Linq;

namespace LowEffort
{
    public class PuzzleDesignerEditor : EditorWindow
    {
        //Store the puzzle dimensions
        int puzzleWidth = 8;
        int puzzleHeight = 8;
        Vector2 currentPuzzleDimensions;

        //Window and puzzle view variables
        static Vector2 windowSize = new Vector2(1200, 950);
        Rect puzzleShapeRect = new Rect(350, 100, 800, 800);
        bool isPuzzleActive;

        int currentToolbarSelection = 0;
        string[] toolbarStrings = { "Puzzle Design", "Piece Design" };

        //Border and piece options
        GUIStyle pieceStyle = new GUIStyle();
        RectOffset border;
        static Texture2D boxBorder;
        static GUIStyle boxStyle = new GUIStyle();
        static string borderName = "OuterBorder_white.png";

        //Store the values of the puzzle pieces and colors
        static string designFolderPath = "Assets/Low Effort Jam 15 Work/Designs/";
        string fileName = "";
        public static PuzzleDesignManager designManager;
        PuzzleDesign design;
        BackingValue[,] resultsArray = new BackingValue[40, 40];
        static List<Color> colors = new List<Color>();
        static int numOfColors = 2;
        static Color currentActiveColor = Color.black;
        static ColorPickerEditor colorPicker;
        int currentColorButtonClicked;

        private void OnEnable()
        {
            colorPicker = new ColorPickerEditor();
            ColorPickerEditor.OnColorSelected += ChangeColor;
        }

        private void OnDisable()
        {
            ColorPickerEditor.OnColorSelected -= ChangeColor;
        }

        private void ChangeColor(Color color)
        {
            colors[currentColorButtonClicked] = color;
            currentActiveColor = color;
        }

        private void OpenColorPicker(int colorSpot)
        {
            currentColorButtonClicked = colorSpot;

            if (colorPicker == null)
                colorPicker = new ColorPickerEditor();

            colorPicker.ShowAuxWindow();
        }

        [MenuItem("Tools/Puzzle Creator")]
        public static void Init()
        {
            PuzzleDesignerEditor designer = (PuzzleDesignerEditor)EditorWindow.GetWindow(typeof(PuzzleDesignerEditor));
            designer.minSize = windowSize;
            designer.maxSize = windowSize;
            designer.titleContent.text = "Puzzle Creator";
            designer.Show();


            //create design manager to store the designs
            designManager = ScriptableObject.CreateInstance<PuzzleDesignManager>();
            string[] result = AssetDatabase.FindAssets("PuzzleDesignManager1");
            Debug.Log(result);
            if(result.Length != 0)
            {
                Debug.Log("Found asset file");
                foreach (PuzzleDesign item in designManager.puzzleDesigns)
                {
                    Debug.Log("item is " + item.ToString());
                }
            }
            else
            {
                Debug.Log("NOT found");
                AssetDatabase.CreateAsset(designManager, designFolderPath + "PuzzleDesignManager1.asset");
            }
            
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

            GUILayout.BeginArea(new Rect(0,110,300,100));
            currentToolbarSelection = GUILayout.Toolbar(currentToolbarSelection, toolbarStrings);
            GUILayout.EndArea();

            //Draw the board
            if (currentToolbarSelection == 0)
            {
                if (!isPuzzleActive)
                    BuildBlankPuzzle();

                DrawSideButtons(e);

                GUILayout.BeginArea(puzzleShapeRect);
                DrawBoard(e);
                GUILayout.EndArea();
            }

            if(currentToolbarSelection == 1)
            {
                //Draw the piece design section
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
            numOfColors = (int)EditorGUILayout.Slider("Number of Colors", numOfColors, 2, 8);
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
            GUILayout.BeginHorizontal(GUILayout.Width(250));
            GUILayout.Label("Design File Name");
            fileName = EditorGUILayout.TextField("", fileName, GUILayout.Width(200));
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
                if(GUI.Button(colorSwatch, "", boxStyle))
                {
                    if (e.button == 0)
                    {
                        currentActiveColor = colors[i];
                    }

                    if (e.button == 1)
                    {
                        OpenColorPicker(i);
                        Repaint();
                    }
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

            ResetColors();
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
                    basePiece.x = resultsArray[i, j].location.x * size;
                    basePiece.y = resultsArray[i, j].location.y * size;
                    EditorGUI.DrawRect(new Rect(basePiece), resultsArray[i,j].color);

                    //Draw the border texture and change the texture color on mouse click
                    GUI.backgroundColor = Color.black;
                    if (GUI.Button(basePiece, "", boxStyle))
                    {
                        if (e.button == 0)
                        {
                            resultsArray[i, j].color = currentActiveColor;
                        }else if(e.button == 1)
                        {
                            resultsArray[i, j].color = Color.clear;
                        }
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
            PuzzleDesign design = new PuzzleDesign();
            design.SaveData(resultsArray, colors);
            design.SetCodeValue(fileName);
            design.SetDimensions(puzzleHeight, puzzleWidth);

            bool result = designManager.puzzleDesigns.FirstOrDefault(t => t.CodeValue == fileName);
            PuzzleDesign item;
            if (result)
            {
                Debug.Log(fileName + " is found");
                item = designManager.puzzleDesigns.FirstOrDefault(t => t.Equals(design));
                item = design;


            }
            else
            {
                AssetDatabase.CreateAsset(design, designFolderPath + fileName + ".asset");
                designManager.puzzleDesigns.Add(design);
                item = design;
            }



            item.SaveData(resultsArray, colors);


            Debug.Log(item.backingValues.Length);

            EditorUtility.SetDirty(designManager);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

        }

        private void LoadFile()
        {

        }

        private static void ResetColors()
        {
            colors.Clear();
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

}
