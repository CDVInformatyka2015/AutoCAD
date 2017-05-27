using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;

namespace ClassLibrary1
{
    public class Class1
    {
        Point3d point, point2;
        Editor editor = Application.DocumentManager.MdiActiveDocument.Editor;

        [CommandMethod("pierwszy")]
        public void pierwszy()
        {
            Application.ShowAlertDialog("AutoCAD to zło");
        }

        [CommandMethod("pobieranie")]
        public void download()
        {
            PromptPointOptions comments = new PromptPointOptions("Podaj pierwszy punkt");
            PromptPointResult result = this.editor.GetPoint(comments);
            this.point = result.Value;

            Application.ShowAlertDialog(point.X.ToString() + "\n" + point.Y.ToString() + "\n" + point.Z.ToString());

            PromptPointOptions comments2 = new PromptPointOptions("Podaj drugi punkt");
            comments2.UseBasePoint = true;
            comments2.BasePoint = point;
            PromptPointResult result2 = this.editor.GetPoint(comments2);

            if (result2.Status == PromptStatus.OK)
            {
                this.point2 = result2.Value;
            }

            Database database = Application.DocumentManager.MdiActiveDocument.Database;
            using (Transaction transaction = database.TransactionManager.StartTransaction())
            {
                Line line1 = new Line(this.point, this.point2);

                BlockTable table = transaction.GetObject(database.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord model = transaction.GetObject(table[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                model.AppendEntity(line1);
                transaction.AddNewlyCreatedDBObject(line1, true);
                transaction.Commit();
            }
        }

    }
}
