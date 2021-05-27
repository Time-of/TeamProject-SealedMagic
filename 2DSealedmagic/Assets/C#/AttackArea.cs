using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
�ۼ�: 20181220 �̼���(P)

�ۼ� �ǵ�: �÷��̾�-���� ��ȣ ���� �ǰ� ��ȣ�ۿ���
�ϰ������� ������ �� �ִ� ��ũ��Ʈ�� ������� �Ͽ���. (������ ��ũ��Ʈ?)

����: '���� ����' �� �ִ� ������Ʈ�� ���� ��ũ��Ʈ�� Trigger�� ������ �۵�

����:	�������� ������Ʈ�� AttackArea ����,
		Ÿ ��ũ��Ʈ�� ���ݺκп��� GetComponent<AttackArea>
		�ʼ�: damage, isEnemyAttack ���� ��
		�����̻��� ���: AttackType, dogDamage or speedModify, duration
		isTrigger�� �� üũ�ϱ� �ٶ�
*/

public class AttackArea : MonoBehaviour
{
	[Tooltip("������ �����̻� �����̶�� � ������ ���ԵǴ��� (Fire, Slow, Stun)")]
	public string AttackType;

	// true�� ��� ���� ����, false�� ��� �÷��̾��� ����
	[HideInInspector] public bool isEnemyAttack;
	// ���� �Լ��κ��� �������� ������
	[HideInInspector] public float damage;
	// ��Ʈ ������ ��� ��Ʈ ���ط�
	[HideInInspector] public float dotDamage;
	// �����̻��� �ӵ��� ������ ��� �ӵ� ������
	[HideInInspector] public float speedModify;
	// �����̻��� ���� �ð�
	[HideInInspector] public float duration;

	void OnTriggerEnter2D(Collider2D collision)
	{
		// �÷��̾��� ������ ���
		if (!isEnemyAttack)
		{
			if (collision.gameObject.tag == "Enemy")
			{
				Monster mob = collision.GetComponent<Monster>();

				if (mob != null)
				{
					mob.onAttack(damage);

					if (AttackType == "Slow")
					{
						mob.modifySpeed(speedModify, duration);
					}
					if (AttackType == "Fire")
					{
						mob.startDotDamage(dotDamage, duration);
					}
					if (AttackType == "Stun")
					{
						mob.startStun(duration);
					}

					Destroy(gameObject);
				}
			}
		}

		// ������ ������ ���
		if (isEnemyAttack)
		{
			if (collision.gameObject.tag == "Player" )
			{
				PlayerObject pl = collision.GetComponent<PlayerObject>();

				if (pl != null)
				{
					pl.OnDamage(damage);

					if (AttackType == "Slow")
					{
						//pl.modifySpeed(speedModify, duration);
					}
					if (AttackType == "Fire")
					{
						//pl.startDotDamage(dotDamage, duration);
					}
					if (AttackType == "Stun")
					{
						//pl.startStun(duration);
					}

					Destroy(gameObject);
				}
			}
		}
	}
}