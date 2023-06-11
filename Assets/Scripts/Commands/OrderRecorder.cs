using System.Collections.Generic;
using UnityEngine;

public class OrderRecorder
{
    Stack<IOrder> _orderList;

    public OrderRecorder()
    {
        _orderList = new Stack<IOrder>();
    }

    public void AddOrder(IOrder newOrder)
    {
        newOrder.Execute();
        _orderList.Push(newOrder);
    }

    public void UndoCommand()
    {
        if (_orderList.Count > 0)
        {
            IOrder latestOrder = _orderList.Pop();
            latestOrder.Undo();
        }
    }
}