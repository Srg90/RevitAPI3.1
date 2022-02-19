using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPI3._1
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            IList<Reference> selectedElementRefList = uidoc.Selection.PickObjects(ObjectType.Face, "Выберете элементы стен");
            var wallList = new List<Wall>();
            var volume = new List<double>();
            double sum = 0;
            string Vol = string.Empty;

            foreach (var selectedElement in selectedElementRefList)
            {
                //Wall oWall = doc.GetElement(selectedElement) as Wall;
                //wallList.Add(oWall);
                Element element = doc.GetElement(selectedElement);
                if (element is Wall)
                {
                    Wall oWall = (Wall)element;
                    wallList.Add(oWall);
                }
            }
            foreach (var wall in wallList)
            {
                Parameter wallVolume = wall.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED);
                double V = UnitUtils.ConvertFromInternalUnits(wallVolume.AsDouble(), UnitTypeId.CubicMeters);
                volume.Add(V);
            }
            foreach (var i in volume)
            {
                sum += i;
            }
            string SumVolume = sum.ToString();
            Vol += $"Объем: {SumVolume}м3{Environment.NewLine}Общее количество элементов стен: {wallList.Count}";

            TaskDialog.Show("Selection", Vol);

            return Result.Succeeded;
        }
    }
}
