using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cameras
{
    public class MainCameraController : MonoBehaviour
    {
        // �v���C���[�I�u�W�F�N�g
        [field: SerializeField] private GameObject _player = null;
        // �J�����̃I�t�Z�b�g
        [field: SerializeField] private Vector3 _offset = Vector3.zero;

        void Start()
        {
            
        }

        void LateUpdate()
        {
            this.transform.position = _player.transform.position + this._offset;
        }
    }
}