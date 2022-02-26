using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Players
{
    public class PlayerMover : MonoBehaviour
    {
        // ���Enum
        public enum State
        {
            Idling,
            Walking,
            Running,
            Crouching,
            CrouchWalking,
            Arrested,
            GameClear,
        }

        // ��ԑJ�ڕϐ�-----------------------------------------------------------------------------------------------------------------------------------------------------------------
        public State _currentState = State.Idling;
        public State currentState
        {
            get => _currentState;
            set
            {
                if (isArrested)
                {
                    return;
                }
                _currentState = value;
            }
        }
        public bool isArrested => currentState == State.Arrested;


        // ����
        IInputEventProvider _playerInput;
        // Rigidbody
        Rigidbody _playerRigidbody;

        // ���x�ϐ�
        [field: SerializeField] float _maxSpeed = 0f;
        private float _currentMaxSpeed = 0f;

        void Start()
        {
            _playerInput = this.GetComponent<IInputEventProvider>();
            _playerRigidbody = this.GetComponent<Rigidbody>();
        }

        // Update-----------------------------------------------------------------------------------------------------------------------------------------------------------------------
        void Update()
        {
            switch (currentState)
            {
                case State.Idling:
                    {
                        // �������
                        if (_playerInput.Run)
                        {
                            // �������Ȃ�
                        }
                        else
                        {
                            // �������Ȃ�
                        }
                        // ���Ⴊ�ݓ���
                        if (_playerInput.Crouch)
                        {
                            Crouch();
                        }
                        else
                        {
                            // �������Ȃ�
                        }
                        if (GameObject.Find("tablet") == null)
                        {
                            GameClear();
                        }
                    }
                    break;
                case State.Walking:
                    {
                        // �������
                        if (_playerInput.Run)
                        {
                            Run();
                        }
                        else
                        {
                            // �������Ȃ�
                        }
                        // ���Ⴊ�ݓ���
                        if (_playerInput.Crouch)
                        {
                            CrouchWalk();
                        }
                        else
                        {
                            // �������Ȃ�
                        }
                    }
                    break;
                case State.Running:
                    {
                        // �������
                        if (_playerInput.Run)
                        {
                            // �������Ȃ�
                        }
                        else
                        {
                            Walk();
                        }
                        // ���Ⴊ�ݓ���
                        if (_playerInput.Crouch)
                        {
                            // �������Ȃ�
                        }
                        else
                        {
                            // �������Ȃ�
                        }
                    }
                    break;
                case State.Crouching:
                    {
                        // �������
                        if (_playerInput.Run)
                        {
                            // �������Ȃ�
                        }
                        else
                        {
                            // �������Ȃ�
                        }
                        // ���Ⴊ�ݓ���
                        if (_playerInput.Crouch)
                        {
                            // �������Ȃ�
                        }
                        else
                        {
                            Idle();
                        }
                    }
                    break;
                case State.CrouchWalking:
                    {
                        // �������
                        if (_playerInput.Run)
                        {
                            // �������Ȃ�
                        }
                        else
                        {
                            // �������Ȃ�
                        }
                        // ���Ⴊ�ݓ���
                        if (_playerInput.Crouch)
                        {
                            // �������Ȃ�
                        }
                        else
                        {
                            Walk();
                        }
                    }
                    break;
                case State.Arrested:
                    {
                        if (UnityEngine.InputSystem.Keyboard.current.rKey.wasPressedThisFrame)
                        {
                            UnityEngine.SceneManagement.SceneManager.LoadScene("Test01");
                        }
                    }
                    break;
                case State.GameClear:
                    {
                        if (UnityEngine.InputSystem.Keyboard.current.rKey.wasPressedThisFrame)
                        {
                            UnityEngine.SceneManagement.SceneManager.LoadScene("Test01");
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        // FixedUpdate-----------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void FixedUpdate()
        {
            switch (currentState)
            {
                case State.Idling:
                    {
                        if (_playerInput.Move != Vector2.zero)
                        {
                            Walk();
                        }
                        else
                        {
                            // �������Ȃ�
                        }
                    }
                    break;
                case State.Walking:
                    {
                        if (_playerInput.Move != Vector2.zero)
                        {
                            ChangeDirection(_playerInput.Move);
                            MoveForward(_playerInput.Move);
                        }
                        else
                        {
                            Idle();
                        }
                    }
                    break;
                case State.Running:
                    {
                        if (_playerInput.Move != Vector2.zero)
                        {
                            ChangeDirection(_playerInput.Move);
                            MoveForward(_playerInput.Move);
                        }
                        else
                        {
                            Idle();
                        }
                    }
                    break;
                case State.Crouching:
                    {
                        if (_playerInput.Move != Vector2.zero)
                        {
                            CrouchWalk();
                        }
                        else
                        {
                            // �������Ȃ�
                        }
                    }
                    break;
                case State.CrouchWalking:
                    {
                        if (_playerInput.Move != Vector2.zero)
                        {
                            ChangeDirection(_playerInput.Move);
                            MoveForward(_playerInput.Move);
                        }
                        else
                        {
                            Crouch();
                        }
                    }
                    break;
                case State.Arrested:
                    {

                    }
                    break;
                case State.GameClear:
                    {
                        
                    }
                    break;
            }
        }


        // ��ԑJ��--------------------------------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// �A�C�h�����O
        /// </summary>
        private void Idle()
        {
            currentState = State.Idling;
        }

        /// <summary>
        /// ����
        /// </summary>
        private void Walk()
        {
            SpeedChange(_maxSpeed / 2.0f);
            currentState = State.Walking;
        }

        /// <summary>
        /// ����
        /// </summary>
        private void Run()
        {
            SpeedChange(_maxSpeed / 1.0f);
            currentState = State.Running;
        }

        /// <summary>
        /// ���Ⴊ��
        /// </summary>
        private void Crouch()
        {
            currentState = State.Crouching;
        }

        /// <summary>
        /// ���Ⴊ�ݕ���
        /// </summary>
        private void CrouchWalk()
        {
            SpeedChange(_maxSpeed / 3.0f);
            currentState = State.CrouchWalking;
        }

        /// <summary>
        /// �߂܂�
        /// </summary>
        private void Arrested()
        {
            currentState = State.Arrested;
        }

        /// <summary>
        /// �Q�[���N���A
        /// </summary>
        private void GameClear()
        {
            currentState = State.GameClear;
        }

        // ���\�b�h--------------------------------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// �����]��
        /// </summary>
        void ChangeDirection(Vector2 _direction)
        {
            // ��������]
            this.transform.rotation = 
                Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(new Vector3(_direction.x, 0, _direction.y), this.transform.up), 0.15f * Time.fixedDeltaTime * 50.0f);
        }

        /// <summary>
        /// ���x�ύX
        /// </summary>
        void SpeedChange(float speed)
        {
            _currentMaxSpeed = speed;
        }

        /// <summary>
        /// ���i
        /// </summary>
        void MoveForward(Vector2 _direction)
        {
            // �ڕW�̑��x
            Vector3 _targetSpeed = new Vector3(_direction.x, 0, _direction.y) * _currentMaxSpeed;

            // �ڕW�̑��x�ƌ��݂̑��x�̍�
            Vector3 applyforce = _targetSpeed - _playerRigidbody.velocity;

            // �㉺�����̑��x�͖�������
            applyforce.y = 0f;

            // ������͂��傫������ꍇ
            if (applyforce.magnitude > _currentMaxSpeed)
            {
                applyforce = applyforce.normalized * _currentMaxSpeed;
            }

            // �͂�������
            _playerRigidbody.AddForce(applyforce, ForceMode.VelocityChange);
        }
    }

}