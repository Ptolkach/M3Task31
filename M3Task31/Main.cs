using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M3Task31
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            IList<Reference> selectedElementRefList = uidoc.Selection.PickObjects(ObjectType.Element, new WallFilter(), "Выберите стены");
            var wallVolumeParamList = new List<Parameter>();

            foreach (var selectedElement in selectedElementRefList)
            {
                Element element = doc.GetElement(selectedElement);
                Wall oWall = (Wall)element;
                wallVolumeParamList.Add(oWall.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED));
            }

            var wallVolumeList_SI = new List<Double>();

            foreach (var vol in wallVolumeParamList)
            {
                wallVolumeList_SI.Add(UnitUtils.ConvertFromInternalUnits(vol.AsDouble(), UnitTypeId.CubicMeters));
            }

            TaskDialog.Show("Объем выбранных стен", $"{wallVolumeList_SI.Sum().ToString()}, м3");


            return Result.Succeeded;
        }
    }
}
