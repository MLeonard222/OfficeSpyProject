using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    public class EnemyMover : MonoBehaviour
    {
        public enum State
        {
            LookAround,
            Walking,
            Warning,
            CatchOut,
            Running,
            Arresting
        }

        // ��ԑJ�ڕϐ�-----------------------------------------------------------------------------------------------------------------------------------------------------------------
        public State _currentState = State.LookAround;
        public State currentState
        {
            get => _currentState;
            set
            {
                if (isArresting)
                {
                    return;
                }
                _currentState = value;
            }
        }
        public bool isArresting => currentState == State.Arresting;

        // NavMeshAgent
        NavMeshAgent _enemyNavMeshAgent;

        // �v���C���[Transform
        [field: SerializeField] Transform _playerTransform;

        // target
        [field: SerializeField] Transform _targetTransform;
        private int _nextTarget = 0;

        // ���b�҂�
        private float _waitAFewSecounds = 0f;

        // �v���C���[�̑{��
        RaycastHit[] _playerHit;
        RaycastHit[] _somethingNoticeHit;
        int _playerMask = 1 << 6;
        float _catchOutDistance = 3.0f;
        float _warnDistance = 5.0f;


        void Start()
        {
            _enemyNavMeshAgent = this.GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            switch (currentState)
            {
                case State.LookAround:
                    {
                        // �ӂ�����n��
                        if (_waitAFewSecounds < 3.0f)
                        {
                            _waitAFewSecounds += Time.deltaTime;
                        }
                        // �����������Ȃ���Ε����o��
                        else
                        {
                            Walk();
                        }
                        // �����C�ɂȂ�Όx�����
                        if (SomethingNotice())
                        {
                            _enemyNavMeshAgent.speed = 0;
                            Warn();
                        }
                        // �v���C���[��������
                        if (PlayerFinder())
                        {
                            _enemyNavMeshAgent.speed = 0;
                            CatchOut();
                        }

                    }break;
                case State.Walking:
                    {
                        // �ڕW�n�_�ɓ��B������ӂ������
                        if (_enemyNavMeshAgent.remainingDistance <= _enemyNavMeshAgent.stoppingDistance)
                        {
                            LookAround();
                        }
                        // �����ɔ������Čx����ԂɂȂ�
                        if (SomethingNotice())
                        {
                            _enemyNavMeshAgent.speed = 0;
                            Warn();
                        }
                        // �v���C���[��������
                        if (PlayerFinder())
                        {
                            _enemyNavMeshAgent.speed = 0;
                            CatchOut();
                        }
                    }break;
                case State.Warning:
                    {
                        // �C�ɂȂ�ꏊ��{������
                        if (_waitAFewSecounds < 0.5f)
                        {
                            if (_waitAFewSecounds == 0f) {
                                _enemyNavMeshAgent.speed = 0f;
                                _enemyNavMeshAgent.destination = _somethingNoticeHit[0].point;
                            }
                            _waitAFewSecounds += Time.deltaTime;
                        }
                        else
                        {
                            _enemyNavMeshAgent.speed = 2.0f;
                        }
                        // �ӂ������
                        if (_enemyNavMeshAgent.remainingDistance <= _enemyNavMeshAgent.stoppingDistance)
                        {
                            LookAround();
                        }
                        // �v���C���[��������
                        if (PlayerFinder())
                        {
                            _enemyNavMeshAgent.speed = 0;
                            CatchOut();
                        }

                    }break;
                case State.CatchOut:
                    {
                        // ����n�߂�
                        if (_waitAFewSecounds < 0.5f)
                        {
                            _waitAFewSecounds += Time.deltaTime;
                        }
                        else
                        {
                            Run();
                        }
                    }
                    break;
                case State.Running:
                    {
                        // ����
                        _enemyNavMeshAgent.speed = 6.0f;
                        _enemyNavMeshAgent.stoppingDistance = 1.0f;
                        _enemyNavMeshAgent.destination = _playerTransform.position;
                        // �v���C���[��߂܂���
                        if (_enemyNavMeshAgent.remainingDistance <= _enemyNavMeshAgent.stoppingDistance)
                        {
                            Arrest();
                        }
                    }break;
                case State.Arresting:
                    {
                        _playerTransform.gameObject.GetComponent<Players.PlayerMover>().currentState = Players.PlayerMover.State.Arrested;
                    }
                    break;
            }
        }

        // ��ԑJ��--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// �ӂ�����n��
        /// </summary>
        private void LookAround()
        {
            currentState = State.LookAround;
            _waitAFewSecounds = 0f;
        }

        /// <summary>
        /// ����
        /// </summary>
        private void Walk()
        {
            // ���̃^�[�Q�b�g�֌����ĕ���
            _nextTarget++;
            _enemyNavMeshAgent.SetDestination(_targetTransform.GetChild((_nextTarget) % _targetTransform.childCount).position);

            currentState = State.Walking;
            _waitAFewSecounds = 0f;
        }

        /// <summary>
        /// �x������
        /// </summary>
        private void Warn()
        {
            currentState = State.Warning;
            _waitAFewSecounds = 0f;
        }

        /// <summary>
        /// ������
        /// </summary>
        private void CatchOut()
        {
            currentState = State.CatchOut;
            _waitAFewSecounds = 0f;
        }

        /// <summary>
        /// ����
        /// </summary>
        private void Run()
        {
            currentState = State.Running;
            _waitAFewSecounds = 0f;
        }

        /// <summary>
        /// �߂܂���
        /// </summary>
        private void Arrest()
        {
            currentState = State.Arresting;
            _waitAFewSecounds = 0f;
        }

        // ���\�b�h------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// �v���C���[��������
        /// </summary>
        /// <returns></returns>
        private bool PlayerFinder()
        {
            _playerHit = Physics.SphereCastAll(this.transform.position + Vector3.up * 1.0f, 0.5f, this.transform.forward, _catchOutDistance);

            if (_playerHit.Length == 1)
            {
                if (_playerHit[0].transform.gameObject.CompareTag("Player"))
                {
                    return true;
                }
            }

            return false;
        }

        private bool SomethingNotice()
        {
            _somethingNoticeHit = Physics.SphereCastAll(this.transform.position + Vector3.up * 1.0f, 0.5f, this.transform.forward, _warnDistance);

            if (_somethingNoticeHit.Length == 1 && _somethingNoticeHit[0].distance > _catchOutDistance)
            {
                if (_somethingNoticeHit[0].transform.gameObject.CompareTag("Player"))
                {
                    return true;
                }
            }

            return false;
        }

        // �M�Y��--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            //if (_playerHit.Length >= 1)
            //{
            //    Gizmos.color = Color.blue;
            //    Gizmos.DrawRay(this.transform.position + Vector3.up * 1.0f, this.transform.forward * _playerHit[0].distance);
            //    Gizmos.DrawWireSphere(this.transform.position + Vector3.up * 1.0f + this.transform.forward * _playerHit[0].distance, 0.5f);
            //}
        }
#endif
    }
}