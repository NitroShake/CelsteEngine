﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelsteEngine
{
    public static class NodeManager
    {
        public static List<VisualNode> visualNodes = new();
        public static List<Collider> colliderNodes = new();
        public static CelsteGame game;
        public static Node3D masterNode;
        public static Camera activeCamera;
        public static DirectionalLight activeLight = null;
    }
}
