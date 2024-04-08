using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelsteEngine
{
    public abstract class Node
    {
        public Node(Vector3 position, Vector3 rotation, bool inheritTransform, List<Node> children, Node? parent)
        {
            load();
            this.position = position;
            this.rotation = rotation;
            this.inheritTransform = inheritTransform;
            this.children = children;
            this.parent = parent;
            if (parent != null)
            {
                parent.addChild(this);
            }
        }
        public Vector3 position;
        public Vector3 rotation;
        bool inheritTransform;
        public Node? parent;
        public List<Node> children;

        internal void update(double deltaTime)
        {
            onUpdate(deltaTime);
            foreach (Node child in children)
            {
                child.update(deltaTime);
            }
        }

        public virtual void onUpdate(double deltaTime) { }
        public virtual void load() { }

        public virtual void addChild(Node node) { }


        /// <summary>
        /// Detaches a child node. This removes the node from the scene tree but does not delete the child node (if you're unsure, use dispose instead)
        /// </summary>
        /// <param name="node"></param>
        public void removeChild(Node node)
        {
            children.Remove(node);
        }

        /// <summary>
        /// Unloads the object, performing any 
        /// </summary>
        internal virtual void dispose() 
        {
            if (parent != null)
            {
                parent.removeChild(this);
            }
            onDispose();
        }

        public virtual void onDispose() { }

    }
}
