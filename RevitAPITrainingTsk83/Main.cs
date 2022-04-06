using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPITrainingTsk83
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        //Задание 8.3 Экспорт в изображение
        //Экспортируйте в качестве изображения план 1 этажа из файла упражнений


        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            ViewPlan viewPlan = new FilteredElementCollector(doc)
                  .OfClass(typeof(ViewPlan))
                  .Cast<ViewPlan>()
                  .FirstOrDefault(v => v.ViewType == ViewType.FloorPlan && v.Name.Equals("Level 1"));//Выбор конкретного плана этажа

            //var viewSets = new List<ViewSet>();

            string desktop_path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            string filepath = Path.Combine(desktop_path,
              viewPlan.Name);

            var imageOptions = new ImageExportOptions
                {

                    ZoomType = ZoomFitType.FitToPage,
                    PixelSize = 1980,
                    FilePath = filepath,
                    //ViewName="exportLevel1",
                    FitDirection = FitDirectionType.Horizontal,
                    HLRandWFViewsFileType = ImageFileType.JPEGLossless,
                    ImageResolution = ImageResolution.DPI_600,
                    //ExportRange = ExportRange.SetOfViews
            }; 
            
            imageOptions.SetViewsAndSheets(new List<ElementId> { viewPlan.Id });


            using (var ts = new Transaction(doc, "export Image"))
            {
                ts.Start();

                doc.ExportImage(imageOptions);
                ts.Commit();
            }

            TaskDialog.Show("Мессадж", "Экспорт завершен. Ищите файл на рабочем столе");

            return Result.Succeeded;
        }
    }
}
