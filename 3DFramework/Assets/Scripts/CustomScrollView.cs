using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CustomScrollView : MonoBehaviour
{
    public GameObject _itemPrefab;      // 재사용 아이템 프리팹
    public RectTransform _viewport;     // 보여지는 영역 (뷰포트)
    public RectTransform _contentRoot;  // 아이템들 담을 영역
    public ScrollRect _scrollRect;      // 스크롤 영역에 붙은 컴포넌트

    public int _itemCount = 2000;       // 전체 개수
    public float _itemHeight = 30f;     // 아이템 하나 높이, TODO : 나중에는 prefab에서 가져오도록 개선
    public int _bufferCount = 10;       // 추가로 생성할 버퍼 아이템 개수

    private List<CustomScrollItem> _pool = new();       // 재사용 가능한 아이템 풀
    private int _visibleCount = 0;                      // 화면에 실제로 보여지는 아이템 개수
    private int _lastFirstIndex = -1;                   // 이전 프레임의 시작 인덱스 (같으면 갱산 X)

    void Start()
    {
        // 뷰포트 기준으로 화면에 보이는 아이템 개수 계산 (버퍼 개수 포함)
        _visibleCount = Mathf.CeilToInt(_viewport.rect.height / _itemHeight) + _bufferCount;

        // 실제로 보여질 만큼 아이템 생성 (풀링)
        for (int i = 0; i < _visibleCount; ++i)
        {
            GameObject item = Instantiate(_itemPrefab, _contentRoot);
            _pool.Add(item.GetComponent<CustomScrollItem>());
        }

        // 전체 아이템 개수에 맞춰서 Content 크기 조정
        _contentRoot.sizeDelta = new Vector2(_contentRoot.sizeDelta.x, _itemHeight * _itemCount);

        // 스크롤 값이 변경될 때만, 갱신되도록 이벤트
        _scrollRect.onValueChanged.AddListener(_ => RefeshVisibleItems());

        RefeshVisibleItems();
    }

    void RefeshVisibleItems()
    {
        // 현재 스크롤 위치 Y 계산
        float scrollY = Mathf.Abs(_contentRoot.anchoredPosition.y);
        int firstIndex = Mathf.FloorToInt(scrollY / _itemHeight);

        if (firstIndex == _lastFirstIndex)
        {
            return;
        }

        _lastFirstIndex = firstIndex;

        // 풀에 있는 갯수만큼 반복, 실제 보이는 아이템만 갱신
        for (int i = 0; i < _pool.Count; ++i)
        {
            int dataIndex = firstIndex + i;

            // 스크롤 범위 밖이면 비활성화
            if (dataIndex < 0 || dataIndex >= _itemCount)
            {
                _pool[i].gameObject.SetActive(false);
                continue;
            }

            _pool[i].gameObject.SetActive(true);

            // 아이템 위치를 직접 배치 (LayoutGroup 사용 X)
            _pool[i].transform.localPosition = new Vector3(_pool[i].transform.localPosition.x, -dataIndex * _itemHeight, 0);
            _pool[i].SetData(dataIndex);
        }
    }
    /*
    void Update()
    {
        float scrollY = Mathf.Abs(_contentRoot.anchoredPosition.y);
        int firstIndex = Mathf.FloorToInt(scrollY / _itemHeight);

        for (int i = 0; i < _pool.Count; ++i)
        {
            int dataIndex = firstIndex + i;
            if (dataIndex < 0 || dataIndex >= _itemCount)
            {
                _pool[i].gameObject.SetActive(false);
                continue;
            }

            _pool[i].gameObject.SetActive(true);
            _pool[i].transform.localPosition = new Vector3(0, -dataIndex * _itemHeight, 0);
            _pool[i].SetData(dataIndex);
        }
    }
    */
}
