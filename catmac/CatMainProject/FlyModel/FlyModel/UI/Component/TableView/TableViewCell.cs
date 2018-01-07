using UnityEngine;
using System.Collections;

namespace FlyModel.UI.Component
{
    /// <summary>
    /// The base class for cells in a TableView. ITableViewDataSource returns pointers
    /// to these objects
    /// </summary>
    public class TableViewCell// : MonoBehaviour
    {
        public GameObject GameObject;

        public TableViewCell(GameObject go)
        {
            GameObject = go;
        }

        /// <summary>
        /// TableView will cache unused cells and reuse them according to their
        /// reuse identifier. Override this to add custom cache grouping logic.
        /// </summary>
        public virtual string reuseIdentifier { 
            get { 
                return this.GetType().Name; 
            } 
        }
    }
}
