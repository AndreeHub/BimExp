using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI.Selection;

namespace MeasureAndCount
{
    [Transaction(TransactionMode.Manual)]
    public class MeasureAndCount : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            //get current project document  
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            //get current selecton  
            Selection selection = uidoc.Selection;

            //get ids from curent selection  
            ICollection<ElementId> selectedIds = selection.GetElementIds();

            string FamilyAndInstanceName;
            double Lenght = 0;
            Dictionary<string, List<Element>> dict = new Dictionary<string, List<Element>>();
            List<string> textForTreeView = new List<string>();

            //populate the dictionary  
            foreach (ElementId id in selectedIds)
            {
                Element ele = doc.GetElement(id);
                ElementId eTypeId = ele.GetTypeId(); //get the id of the elemnt type    
                ElementType eType = doc.GetElement(eTypeId) as ElementType;

                //Add up all the parameters that have the length property
                if (ele.LookupParameter("Length") != null)
                {
                    Parameter paraLength = ele.LookupParameter("Length");
                    Lenght += Math.Round(paraLength.AsDouble() * 304.8, 2);
                    continue;
                }

                //single out fittings that have the same family and type anad instance name but diferent parameter
                //in this case a pipe bend of 45 and 90 degree have a same name but not the same angle parameter
                if (ele.LookupParameter("CAx_Winkel") != null)
                {
                    Parameter paraFittingAngle = ele.LookupParameter("CAx_Winkel");
                    FamilyAndInstanceName = eType.FamilyName.ToString() + ", " + ele.Name.ToString() + ", CAx_Winkel: " + (paraFittingAngle.AsDouble() * Math.PI / 180).ToString();
                }
                else FamilyAndInstanceName = eType.FamilyName.ToString() + ", " + ele.Name.ToString();

                

                //Populate the dictionary with the elements that dont containt he length property
                if (!dict.ContainsKey(FamilyAndInstanceName))
                {
                    dict.Add(FamilyAndInstanceName, new List<Element>());
                }
                dict[FamilyAndInstanceName].Add(ele);   
            }

            textForTreeView.Add("\nLength of elments in selection is : " + Lenght.ToString());
            foreach (var items in dict.Keys)
            {

                textForTreeView.Add("\n" + items + " count: " + dict[items].Count.ToString());
            }

            MeasureAndCountView viewer = new MeasureAndCountView(textForTreeView);
            viewer.ShowDialog();

            return Result.Succeeded;
        }
    }
}
