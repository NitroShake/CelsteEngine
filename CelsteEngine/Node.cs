using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelsteEngine
{
    public class Node
    {
        List<Node> nodesToAdd = new();


        public Node(List<Node> children = null, Node parent = null)
        {
            load();
            if (children == null)
            {
                children = new List<Node>();
            }
            if (parent != null)
            {
                parent.addChild(this);
            }
            foreach (var child in children)
            {
                addChild(child);
            }
        }
        public Node? parent;
        public List<Node> children = new();

        internal void update(double deltaTime)
        {
            onUpdate(deltaTime);
            foreach (Node child in children)
            {
                child.update(deltaTime);
            }
            children.AddRange(nodesToAdd);
            nodesToAdd.Clear();
        }

        public virtual void onUpdate(double deltaTime) { }
        public virtual void load() { }

        public void queueAddNode(Node node)
        {
            nodesToAdd.Add(node);
        }

        public virtual void addChild(Node node) 
        {
            if (node.parent != null && node.parent != this)
            {
                throw new InvalidOperationException("Node already has a parent! Use removeChild(node) first");
            }
            else
            {
                node.parent = this;
                children.Add(node);
            }
        }


        /// <summary>
        /// Detaches a child node. This removes the node from the scene tree but does not delete the child node (if you're unsure, use dispose instead)
        /// </summary>
        /// <param name="node"></param>
        public void removeChild(Node node)
        {
            if (children.Remove(node))
            {
                node.parent = null;
            }
        }

        /// <summary>
        /// Unloads the object, performing any 
        /// </summary>
        public virtual void dispose()
        {
            if (parent != null)
            {
                parent.removeChild(this);
            }
            List<Node> list = new List<Node>(children.ToList());
            foreach(Node child in list)
            {
                child.dispose();
            }
        }
    }
}
