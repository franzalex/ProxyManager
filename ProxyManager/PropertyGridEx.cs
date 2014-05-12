using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

namespace ProxyManager
{
    public class PropertyGridEx : PropertyGrid
    {
        private const int DocCommentHeight = 64;
        private ToolStrip _toolStrip;
        private Control _pgView;
        private Control _hotCommands;
        private Control _docComment;
        private Label _commentTitle;
        private Label _commentDescr;
        private int _commentBoxHeight;

        public PropertyGridEx()
        {
            var fieldFlags = BindingFlags.NonPublic | BindingFlags.Instance;
            _toolStrip = (ToolStrip)base.GetType().BaseType.GetField("toolStrip", fieldFlags).GetValue(this);
            _pgView = (Control)base.GetType().BaseType.GetField("gridView", fieldFlags).GetValue(this);
            _hotCommands = (Control)base.GetType().BaseType.GetField("hotcommands", fieldFlags).GetValue(this);
            _docComment = (Control)base.GetType().BaseType.GetField("doccomment", fieldFlags).GetValue(this);
            _commentTitle = (Label)_docComment.GetType().GetField("m_labelTitle", fieldFlags).GetValue(_docComment);
            _commentDescr = (Label)_docComment.GetType().GetField("m_labelDesc", fieldFlags).GetValue(_docComment);

            _toolStrip.Dock = DockStyle.Top;
            _pgView.Dock = DockStyle.Fill;
            _hotCommands.Dock = DockStyle.Bottom;
            _docComment.Dock = DockStyle.Bottom;
            _docComment.SizeChanged += (o, e) => { if (!this.Capture) _docComment.Height = _commentBoxHeight; };

            this.CommentBoxHeight = DocCommentHeight;
        }

        public System.Drawing.Font CommentTitleFont
        {
            get { return _commentTitle.Font; }
            set { _commentTitle.Font = value; }
        }

        [Category("Appearance"), DefaultValue(DocCommentHeight)]
        public int CommentBoxHeight
        {
            get { return _commentBoxHeight; }
            set
            {
                _commentBoxHeight = value;
                _docComment.Height = value;
            }
        }

        protected override void OnPropertyValueChanged(PropertyValueChangedEventArgs e)
        {
            base.OnPropertyValueChanged(e);
            var i = e.ChangedItem;
            while (i.Parent != null)
                i = i.Parent;

            foreach (GridItem c in i.GridItems)
            {
                c.Expanded = c.Expandable;
            }
        }
    }
}
