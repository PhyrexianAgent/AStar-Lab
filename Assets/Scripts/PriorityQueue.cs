using System.Collections;

public class PriorityQueue
{
    private ArrayList nodes = new ArrayList();

    public int Length() => nodes.Count;
    public bool Contains(object node) => nodes.Contains(node);

    public Node GetFirst(){
        if (nodes.Count == 0)
            return null;
        return (Node)nodes[0];
    }

    public void Add(Node node){
        nodes.Add(node);
        nodes.Sort();
    }

    public void Remove(Node node){ // I am pretty sure removing a node from a list of nodes will keep same order so sorting it is pointless
        nodes.Remove(node);
    }
}
