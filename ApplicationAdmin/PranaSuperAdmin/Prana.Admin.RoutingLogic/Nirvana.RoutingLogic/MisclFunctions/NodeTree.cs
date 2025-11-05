using System.Runtime.InteropServices;


namespace System.Windows.Forms
{
    /// <summary>
    /// Summary description for NodeTree.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2229:ImplementSerializationConstructors"), Serializable]
    [ComVisible(false)]
    public class NodeTree : System.Windows.Forms.TreeNode
    {
        private int iMINVALUE = -1;

        public NodeTree()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public NodeTree(string Tag, string Text)
        {
            this.Key = Tag;
            this.Text = Text;
        }

        #region  node index of tag
        /// <summary>
        /// Return the index of node within teh collection of node within this node, if it dosen't exist returns -1
        /// </summary>
        /// <param name="_strTag">string Tag</param>
        /// <returns>int Index</returns>

        public int IndexOf(string Tag)
        {
            int _iIndex = this.iMINVALUE;
            for (int i = 0; i < this.Nodes.Count; i++)
            {
                if (this[i].Tag.ToString().Trim().Equals(Tag.Trim()))
                {
                    _iIndex = i;
                    break;
                }
            }

            return _iIndex;
        }

        #endregion


        #region node contains Tag
        /// <summary>
        /// Returns true if the Node collection in this node contains the Tag
        /// </summary>
        /// <param name="Tag">string Tag</param>
        /// <returns>bool Contains</returns>
        public bool Contains(string Tag)
        {
            bool _bContains = false;

            if (this.IndexOf(Tag) >= 0)
            {
                _bContains = true;
            }

            return _bContains;
        }

        public bool Contains(System.Windows.Forms.NodeTree Node)
        {
            //			bool _bContains = false ;
            //
            //			if( this.Contains(Node.Tag.ToString()))
            //			{
            //				_bContains = true ;
            //			}
            //
            //			return _bContains ;
            return this.Contains(Node.Tag.ToString());
        }

        #endregion


        #region node add

        public System.Windows.Forms.NodeTree Add(string Tag, string Text)
        {
            System.Windows.Forms.NodeTree _node = new NodeTree();

            _node.Tag = Tag;
            _node.Text = Text;

            this.Nodes.Add(_node);

            return _node;

        }

        public int Add(System.Windows.Forms.NodeTree Node)
        {
            this.Nodes.Add(Node);
            return this.Nodes.IndexOf(Node);
        }

        #endregion


        #region Key

        public string Key
        {
            get
            {
                return (string)this.Tag;
            }

            set
            {
                this.Tag = value;

            }
        }
        #endregion

        #region Expanded

        public bool Expanded
        {
            get
            {
                return this.IsExpanded;
            }
            set
            {
                if (value)
                {
                    this.ExpandAll();
                }
                else
                {
                    this.Collapse();
                }
            }
        }
        #endregion


        #region Node collection

        public System.Windows.Forms.NodeTree this[string Tag]
        {
            get
            {
                return ((System.Windows.Forms.NodeTree)(this[this.IndexOf(Tag)]));
            }
        }
        public System.Windows.Forms.NodeTree this[int Index]
        {
            get
            {
                return ((System.Windows.Forms.NodeTree)(this.Nodes[Index]));
            }
        }

        #endregion


        #region Selection
        public bool Selected
        {
            get
            {
                return this.IsSelected;
            }
            set
            {
                if (value)
                {
                    this.TreeView.SelectedNode = this;
                }
                else
                {
                    this.TreeView.SelectedNode = null;
                }
            }
        }
        #endregion


        #region change at index
        public bool ChangeAtIndex(int Index, string Tag, string Text)
        {
            bool _bDone = false;

            if (Index < this.Nodes.Count)
            {
                this[Index].Tag = Tag;
                this[Index].Text = Text;

                _bDone = true;
            }

            return _bDone;
        }
        #endregion


        #region  parent node
        public new System.Windows.Forms.NodeTree Parent
        {
            get
            {
                return ((System.Windows.Forms.NodeTree)(base.Parent));
            }

        }

        #endregion

        #region  clear nodes int his node collection
        public bool Clear()
        {
            bool _bResult = false;

            try
            {
                for (int i = 0; i < this.Nodes.Count; i++)
                {
                    if (this.Nodes[i].IsSelected)
                    {
                        this.Selected = true;
                    }
                }


                for (int i = this.Nodes.Count; i > 0; i--)
                {
                    this.Nodes.RemoveAt(0);
                }
                _bResult = true;


            }
            catch (Exception)
            {
                _bResult = false;
            }


            return _bResult;
        }

        #endregion


    }
}

