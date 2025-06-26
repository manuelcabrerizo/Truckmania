using UnityEngine;

// TODO: remove this and use State<Bigfoot>
public class BigfootState : State<int>
{
    protected Bigfoot bigfoot;

    public BigfootState(Bigfoot bigfoot)
    {
        this.bigfoot = bigfoot;
    }
}
