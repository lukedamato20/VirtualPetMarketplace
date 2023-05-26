using System.Collections.Generic;
using BehaviorTree;

public class PetTree : Tree
{
    public UnityEngine.Transform[] waypoints;

    public static float speed = 125f;
    public static float fovRange = 300f;
    public static float interactRange = 150f;

    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new Check_InteractRange(transform),
                new Task_Interact(transform),
            }),
            new Sequence(new List<Node>
            {
                new Check_PlayerInRange(transform),
                new Task_GoToPlayer(transform),
            }),
            //new Sequence(new List<Node>
            //{
            //    new Check_PlayerIsInteracting(transform),
            //    new Task_Waiting(transform),
            //}),
            //new Sequence(new List<Node>
            //{
            //    new Check_Needs(transform),
            //    new Task_TalkToPlayer(transform),
            //}),
            new Task_Wander(transform, waypoints),
        });

        return root;
    }
}