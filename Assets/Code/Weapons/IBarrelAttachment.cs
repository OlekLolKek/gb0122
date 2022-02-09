using UnityEngine;


public interface IBarrelAttachment
{
    Transform AttachmentBarrel { get; }
    AudioSource AttachmentAudioSource { get; }
    GameObject Instance { get; }
    bool IsActive { get; set; }

    void Activate();
    void Deactivate();
}