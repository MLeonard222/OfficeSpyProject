using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lightings
{
    public class SpotLightController : MonoBehaviour
    {
        // �v���C���[�I�u�W�F�N�g
        [field: SerializeField] private GameObject _player = null;
        // ���C�g�̃I�t�Z�b�g
        [field: SerializeField] private Vector3 _offset = Vector3.zero;


        void Start()
        {

        }

        void LateUpdate()
        {
            _offset = _player.transform.forward * 0.3f + _player.transform.up * 2.0f;

            this.transform.position = this._player.transform.position + this._offset;
        }
    }
}