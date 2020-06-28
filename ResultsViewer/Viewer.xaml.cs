using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Eoba.Shipyard.ArrangementSimulator.DataTransferObject;
using Eoba.Shipyard.ArrangementSimulator.BusinessComponent.Interface;
using Eoba.Shipyard.ArrangementSimulator.BusinessComponent.Implementation;
using Eoba.Shipyard.ArrangementSimulator.BusinessComponent;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Security.Cryptography;

namespace Eoba.Shipyard.ArrangementSimulator.ResultsViewer
{
    /// <summary>
    /// UserControl1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Viewer : UserControl
    {
        public Viewer()
        {
            InitializeComponent();
        }

        IBlockArrangement mBlockArrangement;
        IDataManagement mDataManagement;
        IResultsManagement mResultsManagement;

        ArrangementResultWithDateDTO mResultsInfo;
        List<BlockDTO> mBlockInfoList;
        List<WorkshopDTO> mWorkshopInfoList;
        List<UnitcellDTO[,]> mArrangementMatrixList;
        Model3DGroup main3DGroup = new Model3DGroup();
        List<string> tempDateIndexList = new List<string>();

        int seletedDateIndex;
        int workshopIndex = 0;

        /// <summary>
        /// BLF 알고리즘 적용시 호출되는 생성자
        /// </summary>
        /// <param name="_resultsInfo"></param>
        /// <param name="_workshopInfoList"></param>
        public Viewer(ArrangementResultWithDateDTO _resultsInfo, List<WorkshopDTO> _workshopInfoList)
        {
            mResultsInfo = new ArrangementResultWithDateDTO();
            mResultsInfo = _resultsInfo;

            mBlockArrangement = new BlockArrangementMgr();
            mDataManagement = new DataManagement();
            mResultsManagement = new ResultsManagement();

            mWorkshopInfoList = new List<WorkshopDTO>();
            mWorkshopInfoList = _workshopInfoList;
            mArrangementMatrixList = new List<UnitcellDTO[,]>();

            int numOfDates;


            numOfDates = mResultsInfo.TotalDailyArragnementedBlockList.Count;


            for (int i = 0; i < numOfDates; i++)
            {
                DateTime tempDate = mResultsInfo.ArrangementStartDate;
                tempDate = tempDate.AddDays(i);

                string strtempDate = tempDate.ToShortDateString();

                tempDateIndexList.Add(strtempDate);
            }

            InitializeComponent();

            lstArrDateList.ItemsSource = tempDateIndexList; 
        }

        /// <summary>
        /// Greedy 알고리즘 적용시 호출되는 생성자
        /// </summary>
        /// <param name="_blockInfoList"></param>
        /// <param name="_workshopInfoList"></param>
        public Viewer(List<BlockDTO> _blockInfoList, List<WorkshopDTO> _workshopInfoList, List<UnitcellDTO[,]> _arrangementMatrixList)
        {
            mBlockArrangement = new BlockArrangementMgr();
            mDataManagement = new DataManagement();
            mResultsManagement = new ResultsManagement();
            mBlockInfoList = new List<BlockDTO>();
            mBlockInfoList = _blockInfoList;
            mWorkshopInfoList = new List<WorkshopDTO>();
            mWorkshopInfoList = _workshopInfoList;
            mArrangementMatrixList = new List<UnitcellDTO[,]>();
            mArrangementMatrixList = _arrangementMatrixList;

            InitializeComponent();

            //btnShow.IsEnabled = true;
        }


        /// <summary>
        /// PlateConfig의 상세를 보여줄 수 있는 3D 형상을 출력하는 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShow_Click(object sender, RoutedEventArgs e)
        {
            //이전 코드
            #region
            ////3차원 가시화
            //HelixResultViewer.Children.Clear();
            //if (main3DGroup.Children.Count == 0)
            //{
            //    //카메라 초기화
            //    PerspectiveCamera myCamera = (PerspectiveCamera)HelixResultViewer.Camera;
            //    myCamera = SetCameraPosition(myCamera, mWorkshopInfoList[workshopIndex]);
            //}
            //else main3DGroup.Children.Clear();


            ////조명 설정
            //var lights = new DefaultLights();
            //HelixResultViewer.Children.Add(lights);

            ////작업장 가시화
            //int row = mArrangementMatrixList[workshopIndex].GetLength(0);
            //int column = mArrangementMatrixList[workshopIndex].GetLength(1);
            //main3DGroup.Children.Add(CreateRectModel(row, column, 1, new Point3D(0, 0, 0), Colors.LightGray));
                        
            ////블록 가시화
            //ModelVisual3D model1 = new ModelVisual3D();
            //for (int i = 0; i < mBlockInfoList.Count; i++)
            //{
            //    //일반 블록은 녹색
            //    Color blockColor = Colors.LightGreen;
            //    blockColor.A = 220;

            //    string[] arrprintedstring = { mBlockInfoList[i].Name, mBlockInfoList[i].Project, mBlockInfoList[i].InitialImportDate.ToShortDateString(), mBlockInfoList[i].InitialExportDate.ToShortDateString() };
            //    //블록 위에 출력되는 정보 확인을 위하여 minSize 계산
            //    double minSize = mBlockInfoList[i].RowCount;
            //    if (mBlockInfoList[i].RowCount > mBlockInfoList[i].ColumnCount) minSize = mBlockInfoList[i].ColumnCount;
            //    minSize = minSize / arrprintedstring[2].Length;
            //    if (minSize > 3.0) minSize = 3.0;

            //    main3DGroup.Children.Add(CreateRectModel(Math.Ceiling(mBlockInfoList[i].RowCount), Math.Ceiling(mBlockInfoList[i].ColumnCount), 10.0, new Point3D(Math.Ceiling(mBlockInfoList[i].LocatedRow), Math.Ceiling(mBlockInfoList[i].LocatedColumn), 1), blockColor, minSize, arrprintedstring));
            //}
            //model1.Content = main3DGroup;
            //HelixResultViewer.Children.Add(model1);
            #endregion


        }


        /// <summary>
        /// 실제로 가시화를 수행하는 이벤트 (BLF 알고리즘에서 날짜 선택시)
        /// 주수헌 수정, 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstArrDateList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            seletedDateIndex = lstArrDateList.SelectedIndex;

            //조회하고자 하는 특정 날짜 index
            int target_date = seletedDateIndex;

            List<BlockDTO> targetBlockList = new List<BlockDTO>();

            //블록배치와 주판 배치에 따라 다른 대상 설정

            //특정 날짜에 배치되어 있는 블록 호출
            
            targetBlockList = mResultsInfo.TotalDailyArragnementedBlockList[seletedDateIndex];

            // 배치 블록 가시화 창 밑의 배치 블록 리스트에 현재 블록 정보 입력하는 코드
            //grdBlockDetails.ItemsSource = targetBlockList;


            //작업장 원점 좌표
            List<double[]> ws = new List<double[]>();
            ws.Add(new double[2] { 135 + 1, 75 });
            ws.Add(new double[2] { 135 + 1, 450 });
            ws.Add(new double[2] { 135 + 1, 718 });
            ws.Add(new double[2] { 0, 0 });

            


            WorkshopDTO tempWorkshopDTO = new WorkshopDTO();
            tempWorkshopDTO.RowCount = 199;
            tempWorkshopDTO.ColumnCount = 809;
            //3차원 가시화
            HelixResultViewer.Children.Clear();
            if (main3DGroup.Children.Count == 0)
            {
                //카메라 초기화
                PerspectiveCamera myCamera = (PerspectiveCamera)HelixResultViewer.Camera;
                myCamera = SetCameraPosition(myCamera, tempWorkshopDTO);
            }
            else main3DGroup.Children.Clear();

            //조명 설정
            var lights = new DefaultLights();
            HelixResultViewer.Children.Add(lights);

            DateTime CurrentDate = mResultsInfo.ArrangementStartDate.AddDays(seletedDateIndex);
            ModelVisual3D model1 = new ModelVisual3D();


            //작업장 가시화
            main3DGroup.Children.Add(CreateRectModel(mWorkshopInfoList[0].RowCount, mWorkshopInfoList[0].ColumnCount, 0, new Point3D(ws[0][0], ws[0][1], 0), Colors.White));
            main3DGroup.Children.Add(CreateRectModel(mWorkshopInfoList[1].RowCount, mWorkshopInfoList[1].ColumnCount, 0, new Point3D(ws[1][0], ws[1][1], 0), Colors.White));
            main3DGroup.Children.Add(CreateRectModel(mWorkshopInfoList[2].RowCount, mWorkshopInfoList[2].ColumnCount, 0, new Point3D(ws[2][0], ws[2][1], 0), Colors.White));
            main3DGroup.Children.Add(CreateRectModel(mWorkshopInfoList[3].RowCount, mWorkshopInfoList[3].ColumnCount, 0, new Point3D(ws[3][0], ws[3][1], 0), Colors.White));
            //도크 가시화
            main3DGroup.Children.Add(CreateRectModel(mWorkshopInfoList[3].RowCount, ws[2][1] - mWorkshopInfoList[3].ColumnCount, 0.000000, new Point3D(0, mWorkshopInfoList[3].ColumnCount, 0), Colors.Gray, 10, new string[4] { "Main Dock", "", "", ""}));
            //입고장(북) 가시화
            //main3DGroup.Children.Add(CreateRectModel(mWorkshopInfoList[0].RowCount, ws[1][1] - ws[0][1] - mWorkshopInfoList[0].ColumnCount, 1, new Point3D(ws[0][0], ws[0][1] + mWorkshopInfoList[0].ColumnCount, 0), Colors.Gray));


            //배치불가구역 가시화
            foreach (WorkshopDTO Workshop in mWorkshopInfoList)
            {
                foreach (ArrangementMatrixInfoDTO Object in Workshop.ArrangementMatrixInfoList)
                {
                    main3DGroup.Children.Add(CreateRectModel(Math.Ceiling(Object.RowCount), Math.Ceiling(Object.ColumnCount), 0, new Point3D(ws[Workshop.Index][0] + Math.Ceiling(Object.RowLocation), ws[Workshop.Index][1] + Math.Ceiling(Object.ColumnLocation), 0), Colors.LightCyan, 3, new string[4] { "NA", "", "", "" }));
                }
            }


            //블록 가시화

            foreach (BlockDTO Block in targetBlockList)
            {
                //리드타임 계산 (출고일 - 입고일)
                TimeSpan ts = Block.ExportDate - Block.ImportDate;
                int Leadtime = ts.Days;
                Leadtime++;

                //실제 반출일 계산 (실제 입고일 + 리드타임)
                DateTime ActualExportDate = Block.ActualImportDate.AddDays(Leadtime - 1);

                //남아 있는 작업일 계산 (실제 반출일 - 현재 날짜)
                TimeSpan ts1 = ActualExportDate - CurrentDate;
                int ResidualTime = ts1.Days;
                ResidualTime++;

                //방향에 따른 가로세로 길이 조정
                double tempRow = Math.Ceiling(Block.RowCount);
                double tempCol = Math.Ceiling(Block.ColumnCount);
                if (Block.Orientation == 1)
                {
                    tempRow = Math.Ceiling(Block.ColumnCount);
                    tempCol = Math.Ceiling(Block.RowCount);
                }


                //일반 블록은 검정 테두리
                Color blockColor = Colors.Black;
                //blockColor.A = 220;

                //당일 입고 블록은 초록색
                if (CurrentDate == Block.ActualImportDate) { blockColor = Colors.LightGreen;  }
                //blockColor.A = 100;

                
                //조건 만족 블록은 파란색
                if (Block.IsRoadSide == true) { blockColor = Colors.Blue; }
                
                //지연 블록은 빨간색
                if (Block.IsDelayed == true) { blockColor = Colors.Red; }

                //출고 블록은 노란색
                if (ResidualTime == 1) { blockColor = Colors.Yellow; }
                //blockColor.A = 100;

                string[] arrprintedstring = { Block.Project, "-" + Block.Name, Block.ImportDate.ToShortDateString().Substring(5), Block.ExportDate.ToShortDateString().Substring(5) };
                //블록 위에 출력되는 정보 확인을 위하여 minSize 계산
                double minSize = Block.RowCount;
                if (Block.RowCount > Block.ColumnCount) minSize = Block.ColumnCount;
                minSize = minSize / arrprintedstring[0].Length;
                if (minSize > 3.0) minSize = 3.0;

                main3DGroup.Children.Add(CreateRectModel(tempRow, tempCol, 0, new Point3D(ws[Block.CurrentLocatedWorkshopIndex][0] + Math.Ceiling(Block.LocatedRow), ws[Block.CurrentLocatedWorkshopIndex][1] + Math.Ceiling(Block.LocatedColumn), 0), blockColor));
                //우선순위 블록은 노란색으로 채우기
                if(Block.IsPrior == true) main3DGroup.Children.Add(CreateRectModel(tempRow - 0.8, tempCol - 0.8, 0, new Point3D(ws[Block.CurrentLocatedWorkshopIndex][0] + Math.Ceiling(Block.LocatedRow) + 0.4, ws[Block.CurrentLocatedWorkshopIndex][1] + Math.Ceiling(Block.LocatedColumn) + 0.4, 0), Colors.Yellow, minSize, arrprintedstring));
                else main3DGroup.Children.Add(CreateRectModel(tempRow-0.8, tempCol-0.8, 0, new Point3D(ws[Block.CurrentLocatedWorkshopIndex][0] + Math.Ceiling(Block.LocatedRow)+0.4, ws[Block.CurrentLocatedWorkshopIndex][1] + Math.Ceiling(Block.LocatedColumn)+0.4, 0), Colors.Silver, minSize, arrprintedstring));
            }


            model1.Content = main3DGroup;
            HelixResultViewer.Children.Add(model1);
        }

        Model3DGroup CreateRectModel(double X_Size, double Y_Size, double Z_Size, Point3D moveDistance, Color color)
        {
            Model3DGroup group = new Model3DGroup();

            Point3D p0 = new Point3D(0, 0, 0);
            Point3D p1 = new Point3D(X_Size, 0, 0);
            Point3D p2 = new Point3D(X_Size, 0, Z_Size);
            Point3D p3 = new Point3D(0, 0, Z_Size);
            Point3D p4 = new Point3D(0, Y_Size, 0);
            Point3D p5 = new Point3D(X_Size, Y_Size, 0);
            Point3D p6 = new Point3D(X_Size, Y_Size, Z_Size);
            Point3D p7 = new Point3D(0, Y_Size, Z_Size);

            Point3D p0_move = new Point3D(p0.X + moveDistance.X, p0.Y + moveDistance.Y, p0.Z + moveDistance.Z);
            Point3D p1_move = new Point3D(p1.X + moveDistance.X, p1.Y + moveDistance.Y, p1.Z + moveDistance.Z);
            Point3D p2_move = new Point3D(p2.X + moveDistance.X, p2.Y + moveDistance.Y, p2.Z + moveDistance.Z);
            Point3D p3_move = new Point3D(p3.X + moveDistance.X, p3.Y + moveDistance.Y, p3.Z + moveDistance.Z);
            Point3D p4_move = new Point3D(p4.X + moveDistance.X, p4.Y + moveDistance.Y, p4.Z + moveDistance.Z);
            Point3D p5_move = new Point3D(p5.X + moveDistance.X, p5.Y + moveDistance.Y, p5.Z + moveDistance.Z);
            Point3D p6_move = new Point3D(p6.X + moveDistance.X, p6.Y + moveDistance.Y, p6.Z + moveDistance.Z);
            Point3D p7_move = new Point3D(p7.X + moveDistance.X, p7.Y + moveDistance.Y, p7.Z + moveDistance.Z);

            group.Children.Add(CreateCubeModel(p0_move, p1_move, p2_move, p3_move, p4_move, p5_move, p6_move, p7_move, color));            

            return group;
        }

        Model3DGroup CreateRectModel(double X_Size, double Y_Size, double Z_Size, Point3D moveDistance, Color color, double textSize, string[] arrtext)
        {
            Model3DGroup group = new Model3DGroup();

            Point3D p0 = new Point3D(0, 0, 0);
            Point3D p1 = new Point3D(X_Size, 0, 0);
            Point3D p2 = new Point3D(X_Size, 0, Z_Size);
            Point3D p3 = new Point3D(0, 0, Z_Size);
            Point3D p4 = new Point3D(0, Y_Size, 0);
            Point3D p5 = new Point3D(X_Size, Y_Size, 0);
            Point3D p6 = new Point3D(X_Size, Y_Size, Z_Size);
            Point3D p7 = new Point3D(0, Y_Size, Z_Size);

            Point3D p0_move = new Point3D(p0.X + moveDistance.X, p0.Y + moveDistance.Y, p0.Z + moveDistance.Z);
            Point3D p1_move = new Point3D(p1.X + moveDistance.X, p1.Y + moveDistance.Y, p1.Z + moveDistance.Z);
            Point3D p2_move = new Point3D(p2.X + moveDistance.X, p2.Y + moveDistance.Y, p2.Z + moveDistance.Z);
            Point3D p3_move = new Point3D(p3.X + moveDistance.X, p3.Y + moveDistance.Y, p3.Z + moveDistance.Z);
            Point3D p4_move = new Point3D(p4.X + moveDistance.X, p4.Y + moveDistance.Y, p4.Z + moveDistance.Z);
            Point3D p5_move = new Point3D(p5.X + moveDistance.X, p5.Y + moveDistance.Y, p5.Z + moveDistance.Z);
            Point3D p6_move = new Point3D(p6.X + moveDistance.X, p6.Y + moveDistance.Y, p6.Z + moveDistance.Z);
            Point3D p7_move = new Point3D(p7.X + moveDistance.X, p7.Y + moveDistance.Y, p7.Z + moveDistance.Z);

            Point3D upper_center0 = new Point3D((p0_move.X + p1_move.X) / 2.0 - textSize * 1.5, (p0_move.Y + p4_move.Y) / 2.0, p6_move.Z + 1.0);
            Point3D upper_center1 = new Point3D((p0_move.X + p1_move.X) / 2.0 - textSize * 0.5 , (p0_move.Y + p4_move.Y) / 2.0, p6_move.Z + 1.0);
            Point3D upper_center2 = new Point3D((p0_move.X + p1_move.X) / 2.0 + textSize * 0.5, (p0_move.Y + p4_move.Y) / 2.0, p6_move.Z + 1.0);
            Point3D upper_center3 = new Point3D((p0_move.X + p1_move.X) / 2.0 + textSize * 1.5, (p0_move.Y + p4_move.Y) / 2.0, p6_move.Z + 1.0);

            group.Children.Add(CreateCubeModel(p0_move, p1_move, p2_move, p3_move, p4_move, p5_move, p6_move, p7_move, color));

            if (arrtext != null)
            {
                group.Children.Add(CreateTextLabelModel3D(arrtext[0], Brushes.Black, true, textSize, upper_center0, new Vector3D(0, 1, 0), new Vector3D(-1, 0, 0)));
                group.Children.Add(CreateTextLabelModel3D(arrtext[1], Brushes.Black, true, textSize, upper_center1, new Vector3D(0, 1, 0), new Vector3D(-1, 0, 0)));
                group.Children.Add(CreateTextLabelModel3D(arrtext[2], Brushes.Black, true, textSize, upper_center2, new Vector3D(0, 1, 0), new Vector3D(-1, 0, 0)));
                group.Children.Add(CreateTextLabelModel3D(arrtext[3], Brushes.Black, true, textSize, upper_center3, new Vector3D(0, 1, 0), new Vector3D(-1, 0, 0)));
                //if (arrtext[0].Length != 0) group.Children.Add(bitmapText(arrtext[0], Brushes.Black, true, textSize, upper_center0, new Vector3D(0, 1, 0), new Vector3D(-1, 0, 0)));
                //if (arrtext[1].Length != 0) group.Children.Add(bitmapText(arrtext[1], Brushes.Black, true, textSize, upper_center1, new Vector3D(0, 1, 0), new Vector3D(-1, 0, 0)));
                //if (arrtext[2].Length != 0) group.Children.Add(bitmapText(arrtext[2], Brushes.Black, true, textSize, upper_center2, new Vector3D(0, 1, 0), new Vector3D(-1, 0, 0)));
                //if (arrtext[3].Length != 0) group.Children.Add(bitmapText(arrtext[3], Brushes.Black, true, textSize, upper_center3, new Vector3D(0, 1, 0), new Vector3D(-1, 0, 0)));
            }

            return group;
        }

        public static GeometryModel3D CreateTextLabelModel3D(string text, Brush textColor, bool isDoubleSided, double height, Point3D center, Vector3D textDirection, Vector3D updirection)
        {
            // First we need a textblock containing the text of our label
            var tb = new TextBlock(new Run(text)) { Foreground = textColor, FontFamily = new FontFamily("Arial") };
            tb.FontWeight = FontWeights.Bold;

            // Now use that TextBlock as the brush for a material
            var mat = new DiffuseMaterial { Brush = new VisualBrush(tb) };

            // We just assume the characters are square
            double width = text.Length * height*0.6;

            // tb.Measure(new Size(text.Length * height * 2, height * 2));
            // width=tb.DesiredSize.Width;

            // Since the parameter coming in was the center of the label,
            // we need to find the four corners
            // p0 is the lower left corner
            // p1 is the upper left
            // p2 is the lower right
            // p3 is the upper right
            var p0 = center - width / 2 * textDirection - height / 2 * updirection;
            var p1 = p0 + updirection * 1 * height;
            var p2 = p0 + textDirection * width;
            var p3 = p0 + updirection * 1 * height + textDirection * width;

            // Now build the geometry for the sign.  It's just a
            // rectangle made of two triangles, on each side.
            var mg = new MeshGeometry3D { Positions = new Point3DCollection { p0, p1, p2, p3 } };

            if (isDoubleSided)
            {
                mg.Positions.Add(p0); // 4
                mg.Positions.Add(p1); // 5
                mg.Positions.Add(p2); // 6
                mg.Positions.Add(p3); // 7
            }

            mg.TriangleIndices.Add(0);
            mg.TriangleIndices.Add(3);
            mg.TriangleIndices.Add(1);
            mg.TriangleIndices.Add(0);
            mg.TriangleIndices.Add(2);
            mg.TriangleIndices.Add(3);

            if (isDoubleSided)
            {
                mg.TriangleIndices.Add(4);
                mg.TriangleIndices.Add(5);
                mg.TriangleIndices.Add(7);
                mg.TriangleIndices.Add(4);
                mg.TriangleIndices.Add(7);
                mg.TriangleIndices.Add(6);
            }

            // These texture coordinates basically stretch the
            // TextBox brush to cover the full side of the label.
            mg.TextureCoordinates.Add(new Point(0, 1));
            mg.TextureCoordinates.Add(new Point(0, 0));
            mg.TextureCoordinates.Add(new Point(1, 1));
            mg.TextureCoordinates.Add(new Point(1, 0));

            if (isDoubleSided)
            {
                mg.TextureCoordinates.Add(new Point(1, 1));
                mg.TextureCoordinates.Add(new Point(1, 0));
                mg.TextureCoordinates.Add(new Point(0, 1));
                mg.TextureCoordinates.Add(new Point(0, 0));
            }

            // And that's all.  Return the result.
            return new GeometryModel3D(mg, mat);
        }

        public static GeometryModel3D bitmapText(string _text, Brush textColor, bool isDoubleSided, double height, Point3D center, Vector3D textDirection, Vector3D updirection)
        {
            //Image myImage = new Image();
            FormattedText text = new FormattedText(_text,
                    new CultureInfo("en-us"),
                    FlowDirection.LeftToRight,
                    new Typeface(new FontFamily("Arial"), FontStyles.Normal, FontWeights.Normal, new FontStretch()),
                    height,
                    textColor);

            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            drawingContext.DrawText(text, new Point(0, 0));
            drawingContext.Close();

            // We just assume the characters are square
            double width = _text.Length * height * 0.6;
            int _height = Convert.ToInt32(Math.Floor(height));
            int _width = Convert.ToInt32(Math.Floor(width));
            if (_height == 0) _height = 1;
            if (_width == 0) _width = 1;

            RenderTargetBitmap bmp = new RenderTargetBitmap(_width, _height, 240, 192, PixelFormats.Pbgra32);
            bmp.Render(drawingVisual);
            //myImage.Source = bmp;



            // Now use that TextBlock as the brush for a material
            var mat = new DiffuseMaterial { Brush = new ImageBrush(bmp) { Viewport = new Rect(0,0,1,1), Stretch = Stretch.Fill } };


            // tb.Measure(new Size(text.Length * height * 2, height * 2));
            // width=tb.DesiredSize.Width;

            // Since the parameter coming in was the center of the label,
            // we need to find the four corners
            // p0 is the lower left corner
            // p1 is the upper left
            // p2 is the lower right
            // p3 is the upper right
            var p0 = center - width / 2 * textDirection - height / 2 * updirection;
            var p1 = p0 + updirection * 1 * height;
            var p2 = p0 + textDirection * width;
            var p3 = p0 + updirection * 1 * height + textDirection * width;

            // Now build the geometry for the sign.  It's just a
            // rectangle made of two triangles, on each side.
            var mg = new MeshGeometry3D { Positions = new Point3DCollection { p0, p1, p2, p3 } };

            if (isDoubleSided)
            {
                mg.Positions.Add(p0); // 4
                mg.Positions.Add(p1); // 5
                mg.Positions.Add(p2); // 6
                mg.Positions.Add(p3); // 7
            }

            mg.TriangleIndices.Add(0);
            mg.TriangleIndices.Add(3);
            mg.TriangleIndices.Add(1);
            mg.TriangleIndices.Add(0);
            mg.TriangleIndices.Add(2);
            mg.TriangleIndices.Add(3);

            if (isDoubleSided)
            {
                mg.TriangleIndices.Add(4);
                mg.TriangleIndices.Add(5);
                mg.TriangleIndices.Add(7);
                mg.TriangleIndices.Add(4);
                mg.TriangleIndices.Add(7);
                mg.TriangleIndices.Add(6);
            }

            // These texture coordinates basically stretch the
            // TextBox brush to cover the full side of the label.
            mg.TextureCoordinates.Add(new Point(0, 1));
            mg.TextureCoordinates.Add(new Point(0, 0));
            mg.TextureCoordinates.Add(new Point(1, 1));
            mg.TextureCoordinates.Add(new Point(1, 0));

            if (isDoubleSided)
            {
                mg.TextureCoordinates.Add(new Point(1, 1));
                mg.TextureCoordinates.Add(new Point(1, 0));
                mg.TextureCoordinates.Add(new Point(0, 1));
                mg.TextureCoordinates.Add(new Point(0, 0));
            }

            // And that's all.  Return the result.
            return new GeometryModel3D(mg, mat);
        }

        Model3DGroup CubeParallelMove(double CubeSize, Point3D moveDistance, Color color)
        {
            Model3DGroup group = new Model3DGroup();

            Point3D p0 = new Point3D(0, 0, 0);
            Point3D p1 = new Point3D(CubeSize, 0, 0);
            Point3D p2 = new Point3D(CubeSize, 0, CubeSize);
            Point3D p3 = new Point3D(0, 0, CubeSize);
            Point3D p4 = new Point3D(0, CubeSize, 0);
            Point3D p5 = new Point3D(CubeSize, CubeSize, 0);
            Point3D p6 = new Point3D(CubeSize, CubeSize, CubeSize);
            Point3D p7 = new Point3D(0, CubeSize, CubeSize);

            Point3D p0_move = new Point3D(p0.X + moveDistance.X, p0.Y + moveDistance.Y, p0.Z + moveDistance.Z);
            Point3D p1_move = new Point3D(p1.X + moveDistance.X, p1.Y + moveDistance.Y, p1.Z + moveDistance.Z);
            Point3D p2_move = new Point3D(p2.X + moveDistance.X, p2.Y + moveDistance.Y, p2.Z + moveDistance.Z);
            Point3D p3_move = new Point3D(p3.X + moveDistance.X, p3.Y + moveDistance.Y, p3.Z + moveDistance.Z);
            Point3D p4_move = new Point3D(p4.X + moveDistance.X, p4.Y + moveDistance.Y, p4.Z + moveDistance.Z);
            Point3D p5_move = new Point3D(p5.X + moveDistance.X, p5.Y + moveDistance.Y, p5.Z + moveDistance.Z);
            Point3D p6_move = new Point3D(p6.X + moveDistance.X, p6.Y + moveDistance.Y, p6.Z + moveDistance.Z);
            Point3D p7_move = new Point3D(p7.X + moveDistance.X, p7.Y + moveDistance.Y, p7.Z + moveDistance.Z);

            group.Children.Add(CreateCubeModel(p0_move, p1_move, p2_move, p3_move, p4_move, p5_move, p6_move, p7_move, color));

            return group;
        }
        Model3DGroup CreateCubeModel(Point3D p0, Point3D p1, Point3D p2, Point3D p3, Point3D p4, Point3D p5, Point3D p6, Point3D p7, Color color)
        {
            Model3DGroup group = new Model3DGroup();

            //front side triangles
            group.Children.Add(CreateTriangleModel(p3, p2, p6, color));
            group.Children.Add(CreateTriangleModel(p3, p6, p7, color));
            //right side triangles
            group.Children.Add(CreateTriangleModel(p2, p1, p5, color));
            group.Children.Add(CreateTriangleModel(p2, p5, p6, color));
            //back side triangles
            group.Children.Add(CreateTriangleModel(p1, p0, p4, color));
            group.Children.Add(CreateTriangleModel(p1, p4, p5, color));
            //left side triangles
            group.Children.Add(CreateTriangleModel(p0, p3, p7, color));
            group.Children.Add(CreateTriangleModel(p0, p7, p4, color));
            //top side triangles
            group.Children.Add(CreateTriangleModel(p7, p6, p5, color));
            group.Children.Add(CreateTriangleModel(p7, p5, p4, color));
            //bottom side triangles
            group.Children.Add(CreateTriangleModel(p2, p3, p0, color));
            group.Children.Add(CreateTriangleModel(p2, p0, p1, color));
            return group;
        }
        Model3DGroup CreateTriangleModel(Point3D p0, Point3D p1, Point3D p2, Color color)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();

            mesh.Positions.Add(p0);
            mesh.Positions.Add(p1);
            mesh.Positions.Add(p2);

            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(2);

            Vector3D normal = CalculateNormal(p0, p1, p2);

            mesh.Normals.Add(normal);

            SolidColorBrush brush = new SolidColorBrush(color);
            brush.Opacity = 100;
            Material material = new DiffuseMaterial(brush);
            GeometryModel3D model = new GeometryModel3D(mesh, material);
            Model3DGroup group = new Model3DGroup();
            group.Children.Add(model);

            return group;

        }
        Vector3D CalculateNormal(Point3D p0, Point3D p1, Point3D p2)
        {
            Vector3D v0 = new Vector3D(p1.X - p0.X, p1.Y - p0.Y, p1.Z - p0.Z);
            Vector3D v1 = new Vector3D(p2.X - p0.X, p2.Y - p0.Y, p2.Z - p0.Z);
            return Vector3D.CrossProduct(v0, v1);
        }

        private void btnChangeWorkshop_Click(object sender, RoutedEventArgs e)
        {   
            workshopIndex = Convert.ToInt32(txtWorkshopIndex.Text);

            if ((workshopIndex >= mWorkshopInfoList.Count) || (workshopIndex < 0))
            {
                MessageBox.Show("작업장 번호를 잘못 입력하였습니다.");
            }
            else
            {
                seletedDateIndex = lstArrDateList.SelectedIndex;

                //조회하고자 하는 특정 날짜 index
                int target_date = seletedDateIndex;

                //Grid에 상세 정보 출력
                List<BlockDTO> targetBlockList = new List<BlockDTO>();

                for (int i = 0; i < mResultsInfo.TotalDailyArragnementedBlockList[seletedDateIndex].Count; i++)
                {
                    if (mResultsInfo.TotalDailyArragnementedBlockList[seletedDateIndex][i].CurrentLocatedWorkshopIndex == workshopIndex) targetBlockList.Add(mResultsInfo.TotalDailyArragnementedBlockList[seletedDateIndex][i]);
                }


                //3차원 가시화
                HelixResultViewer.Children.Clear();
                main3DGroup.Children.Clear();

                //카메라 초기화
                PerspectiveCamera myCamera = (PerspectiveCamera)HelixResultViewer.Camera;
                myCamera = SetCameraPosition(myCamera, mWorkshopInfoList[workshopIndex]);

                //조명 설정
                var lights = new DefaultLights();
                HelixResultViewer.Children.Add(lights);

                //작업장 가시화
                main3DGroup.Children.Add(CreateRectModel(mWorkshopInfoList[workshopIndex].RowCount, mWorkshopInfoList[workshopIndex].ColumnCount, 0, new Point3D(0, 0, 0), Colors.White));

                DateTime CurrentDate = mResultsInfo.ArrangementStartDate.AddDays(seletedDateIndex);
                ModelVisual3D model1 = new ModelVisual3D();

                //배치불가구역 가시화

                foreach (ArrangementMatrixInfoDTO Object in mWorkshopInfoList[workshopIndex].ArrangementMatrixInfoList)
                {
                    main3DGroup.Children.Add(CreateRectModel(Math.Ceiling(Object.RowCount), Math.Ceiling(Object.ColumnCount), 0, new Point3D(Math.Ceiling(Object.RowLocation), Math.Ceiling(Object.ColumnLocation), 0), Colors.LightCyan, 3, new string[4] { "NotAvailable", "", "", "" }));
                }



                //블록 가시화

                foreach (BlockDTO Block in targetBlockList)
                {
                    //리드타임 계산 (출고일 - 입고일)
                    TimeSpan ts = Block.ExportDate - Block.ImportDate;
                    int Leadtime = ts.Days;
                    Leadtime++;

                    //실제 반출일 계산 (실제 입고일 + 리드타임)
                    DateTime ActualExportDate = Block.ActualImportDate.AddDays(Leadtime - 1);

                    //남아 있는 작업일 계산 (실제 반출일 - 현재 날짜)
                    TimeSpan ts1 = ActualExportDate - CurrentDate;
                    int ResidualTime = ts1.Days;
                    ResidualTime++;

                    //방향에 따른 가로세로 길이 조정
                    double tempRow = Math.Ceiling(Block.RowCount);
                    double tempCol = Math.Ceiling(Block.ColumnCount);
                    if (Block.Orientation == 1)
                    {
                        tempRow = Math.Ceiling(Block.ColumnCount);
                        tempCol = Math.Ceiling(Block.RowCount);
                    }


                    //일반 블록은 검정 테두리
                    Color blockColor = Colors.Black;
                    //blockColor.A = 220;

                    //당일 입고 블록은 초록색
                    if (CurrentDate == Block.ActualImportDate) { blockColor = Colors.Green; }
                    //blockColor.A = 100;


                    //조건 만족 블록은 파란색
                    if (Block.IsRoadSide == true) { blockColor = Colors.Blue; }

                    //지연 블록은 빨간색
                    if (Block.IsDelayed == true) { blockColor = Colors.Red; }

                    //출고 블록은 노란색
                    if (ResidualTime == 1) { blockColor = Colors.Yellow; }
                    //blockColor.A = 100;

                    string[] arrprintedstring = { Block.Project, "-" + Block.Name, Block.ImportDate.ToShortDateString().Substring(5), Block.ExportDate.ToShortDateString().Substring(5) };
                    //블록 위에 출력되는 정보 확인을 위하여 minSize 계산
                    double minSize = Block.RowCount;
                    if (Block.RowCount > Block.ColumnCount) minSize = Block.ColumnCount;
                    minSize = minSize / arrprintedstring[0].Length;
                    if (minSize > 3.0) minSize = 3.0;

                    main3DGroup.Children.Add(CreateRectModel(tempRow, tempCol, 0, new Point3D(Math.Ceiling(Block.LocatedRow), Math.Ceiling(Block.LocatedColumn), 0), blockColor));
                    main3DGroup.Children.Add(CreateRectModel(tempRow - 0.8, tempCol - 0.8, 0, new Point3D(Math.Ceiling(Block.LocatedRow) + 0.4, Math.Ceiling(Block.LocatedColumn) + 0.4, 0), Colors.Silver, minSize, arrprintedstring));
                }
                model1.Content = main3DGroup;
                HelixResultViewer.Children.Add(model1);
            }
        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            //카메라 초기화
            PerspectiveCamera myCamera = (PerspectiveCamera)HelixResultViewer.Camera;
            myCamera = SetCameraPosition(myCamera, mWorkshopInfoList[workshopIndex]);
        }

        PerspectiveCamera SetCameraPosition(PerspectiveCamera _camera, WorkshopDTO _workshop) 
        {
            PerspectiveCamera returnCamera = _camera;

            double pX = _workshop.RowCount / 2.0;
            double pY = _workshop.ColumnCount / 2.0;
            double pZ;
            if ((pY / pX) < 2.0) //작업장 세로길이(pX)를 기준으로 카메라 맞춤
            {
                pZ = Math.Tan(Math.PI * 80.0 / 180.0) * pX;
            }
            else //작업장 가로길이(pY)를 기준으로 카메라 맞춤
            {
                pZ = Math.Tan(Math.PI * 70.0 / 180.0) * pY;
            }
            

            double dX = 0.0;
            double dY = 0.0;
            double dZ = -1.0;

            double uX = -1.0;
            double uY = 0.0;
            double uZ = 0.0;

            Point3D cameraPosition = new Point3D(pX, pY, pZ);
            Vector3D cameraLookDirection = new Vector3D(dX, dY, dZ);
            Vector3D cameraUpDirection = new Vector3D(uX, uY, uZ);

            returnCamera.Position = cameraPosition;
            returnCamera.LookDirection = cameraLookDirection;
            returnCamera.UpDirection = cameraUpDirection;

            return returnCamera;
        }

        private void txtWorkshopIndex_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }


        
    }
}
