using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Players
{
    public interface IInputEventProvider
    {
        // �ړ��ϐ�
        public Vector2 Move { get; set; }

        // �����L����
        public bool Run { get; set; }

        // ���Ⴊ��
        public bool Crouch { get; set; }

        // �C���^���N�g
        public bool Interact { get; set; }
    }
}