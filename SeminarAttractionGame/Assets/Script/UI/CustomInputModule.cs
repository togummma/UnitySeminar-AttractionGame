using UnityEngine;
using UnityEngine.EventSystems;

public class CustomInputModule : StandaloneInputModule
{
    protected override void ProcessMove(PointerEventData pointerEvent)
    {
        // 既存の入力を取得
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // D-Padの入力を追加
        horizontal += Input.GetAxis("DPadHorizontal");
        vertical += Input.GetAxis("DPadVertical");

        // 結合した入力でUI操作
        Vector2 move = new Vector2(horizontal, vertical);
        pointerEvent.delta = move;
        base.ProcessMove(pointerEvent);
    }
}
