using Photon.Pun;
using UnityEngine;


public sealed class TracerView : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] private LineRenderer _lineRenderer;

    public LineRenderer LineRenderer => _lineRenderer;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //Debug.Log("OnPhotonSerializeView 1 11 1 1");
        if (stream.IsWriting)
        {
            stream.SendNext(_lineRenderer.endWidth);
            SendColor(stream);
            SendPositions(stream);
        }
        else
        {
            _lineRenderer.endWidth = _lineRenderer.startWidth = (float)stream.ReceiveNext();
            ReceiveColor(stream);
            ReceivePositions(stream);
        }
    }

    private void ReceivePositions(PhotonStream stream)
    {
        _lineRenderer.SetPosition(0, (Vector3)stream.ReceiveNext());
        _lineRenderer.SetPosition(1, (Vector3)stream.ReceiveNext());
    }

    private void SendPositions(PhotonStream stream)
    {
        stream.SendNext(_lineRenderer.GetPosition(0));
        stream.SendNext(_lineRenderer.GetPosition(1));
    }

    private void SendColor(PhotonStream stream)
    {
        var color = _lineRenderer.material.color;
        stream.SendNext(color.r);
        stream.SendNext(color.g);
        stream.SendNext(color.b);
        stream.SendNext(color.a);
    }

    private void ReceiveColor(PhotonStream stream)
    {
        var color = new Color
        {
            r = (float)stream.ReceiveNext(),
            g = (float)stream.ReceiveNext(),
            b = (float)stream.ReceiveNext(),
            a = (float)stream.ReceiveNext()
        };
        _lineRenderer.material.color = color;
    }
}