using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickGraph;
using Autodesk.DesignScript.Geometry;
using QuickGraph.Algorithms;

namespace DynamoGraph
{
    public static class CreateGraphs
    {
        public static List<GraphEdge> CompleteGraphBetweenPoints(IEnumerable<Point> allPoints)
        {
            List<GraphEdge> edges = new List<GraphEdge>();
            HashSet<Point> allUniquePoints = new HashSet<Point>(allPoints);

            foreach(Point p in allPoints)
            {
                allUniquePoints.Remove(p);
                foreach(Point p2 in allUniquePoints)
                {
                    edges.Add(new GraphEdge(p, p2));
                }
            }
            return edges;
        }
        
        public static Line LineFromGraphEdge(GraphEdge edge)
        {
            Line line = Line.ByStartPointEndPoint(edge.Source, edge.Target);
            return line;
        }
        
        public static IEnumerable<GraphEdge> MinimumSpanningTreeFromEdges(IEnumerable<GraphEdge> edges, bool useEffectiveLength = false)
        {
            List<Point> unspannedPoints = SpannedPointsOfEdges(edges);
            List<Point> spannedPoints = new List<Point>();

            Point firstPoint = unspannedPoints.First();
            spannedPoints.Add(firstPoint);
            unspannedPoints.Remove(firstPoint);
            
            List<GraphEdge> sortedEdges = SortEdgesByLength(new List<GraphEdge>( edges), useEffectiveLength);
            
            List<GraphEdge> mst = new List<GraphEdge>();
            
            while ( unspannedPoints.Count() > 0)
            {
                GraphEdge newEdge = SmallestEdgeBetweenSetsOfPoints(sortedEdges, spannedPoints, unspannedPoints);
                mst.Add(newEdge);
                sortedEdges.Remove(newEdge);
                unspannedPoints = TrimPointsFromListByEdge(unspannedPoints, newEdge);
                spannedPoints = SpannedPointsOfEdges(mst);
            }
            return mst;
        }

        public static List<Point> TrimPointsFromListByEdge(List<Point> points, GraphEdge edge)
        {
            try
            {
                points.Remove(edge.Source);
                points.Remove(edge.Target);
            }
            catch { }
            return points;
        }

        public static List<GraphEdge> SortEdgesByLength(List<GraphEdge> edges, bool useEffectiveLength = false)
        {
            if (useEffectiveLength)
            {
                return edges.OrderBy(x => x.EffectiveLength).Select(x => x).ToList();
            }
            else {
                return edges.OrderBy(x => x.Length()).Select(x => x).ToList();
            }
        }

        

        public static GraphEdge SmallestEdgeBetweenSetsOfPoints(IEnumerable<GraphEdge> sortedEdges, List<Point> spannedPoints, List<Point> unspannedPoints)
        {
            foreach (GraphEdge e in sortedEdges)
            {
                bool edgeFromTreeToUnSpannedPoint = (spannedPoints.Contains(e.Source) && unspannedPoints.Contains(e.Target));
                bool edgeFromUnspannedPointToTree = (unspannedPoints.Contains(e.Source) && spannedPoints.Contains(e.Target));
                if (edgeFromTreeToUnSpannedPoint || edgeFromUnspannedPointToTree)
                {
                    return e;
                }
            }
            return null;
        }
        
        public static List<Point> SpannedPointsOfEdges(IEnumerable<GraphEdge> edges)
        {
            if (edges.Count() > 0)
            {
                return edges.SelectMany(x => new List<Point> { x.Source, x.Target }).Distinct().ToList();
            }
            else
            {
                return new List<Point>();
            }
        }

        public static GraphEdge SetGraphEdgeEffectiveLength(GraphEdge edge, double effLength)
        {
            edge.EffectiveLength = effLength;
            return edge;
        }
    }

    
    public class GraphEdge
    {
        public Point Source { get; set; }
        public Point Target { get; set; }
        public double EffectiveLength { get; set; }

        public GraphEdge(Point point1, Point point2)
        {
            Source = point1;
            Target = point2;
        }

        public double Length()
        {
            double len = Source.DistanceTo(Target);
            EffectiveLength = len;
            return len;
        }


    }
}
