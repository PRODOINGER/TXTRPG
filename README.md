# -TXTRPG-


　
# 정확히 구현 및 개선 : ● 　　비슷하게 구현 : ▲ 　　구현하지 못함 : X
　
# 필수 기능
# 1. 게임 시작 화면　●        
# 2. 상태보기　● 
# 3. 인벤토리　●
# 4. 장착관리　▲  현재 중복 장착 기능은 도전 과제의 요구 사항을 따라감
# 5. 상점　●
# 6. 아이템 구매　●
　
# 도전 기능
# 1. 아이템 정보를 클래스 / 구조체로 활용해 보기　▲  클래스까지는 활용 하였으나, 배열+구조체 구조가 아닌 List<> 구조를 사용해서 코드 완성 후 변경하지 못 하였음
# 2. 아이템 정보를 배열로 관리하기　X   
# 3. 아이템 추가하기　● 
# 4. 휴식 기능　▲  골드 소모 없이 바로 체력이 회복 됨 
# 5. 아이템 판매　X  
# 6. 장착 개선 - 중복장착 불가　● 
# 7. 레벨업 기능　●
# 8. 던전입장　▲  던전에 입장해서 몬스터와 전투 후 골드를 획득
# 9. 저장하기　●
#  　
# 과제를 구현하며 어려웠던 점
# 대부분 ChatGPT4.0으로 검색하여 구현했기 때문에, 프로젝트는 잘 돌아갔습니다. 
# 그런데 만들어진 코드 중에 제가 처음 보는 단어들이 너무 많았고, 그렇기에 문장의 동작원리를 모르는 경우가 많았습니다.
# 필수 기능 구현 후 과제 제출까지 시간이 있었기에, ChatGPT4.0 검색 없이 기존 기능들을 도전과제 세부사항에 맞게 커스텀하려고 시도했으나 몇 가지를 제외하고는 힘들었습니다.
# 그래서 또 다시 ChatGPT4.0으로 코드 문자들의 동작원리를 적은 상세한 주석을 검색했습니다.
# 이쯤되니 이 프로젝트의 개발자는 제가 아니라 ChatGPT4.0이고 저는 단순 타이핑 툴과 인터페이스 디자이너가 되어 버렸습니다.
#  
　
# 프로젝트 진행과정 ​

# 23일 19시 : TXT RPG 프로젝트 파일 생성
# 24일  01시 : 시작화면 구현 완료
# - 문제점 : 첫 기능 완성까지 5시간 소요 후, 그 다음 기능 구현 방안이 떠오르지 않음
# - 원인 : 능력 부족
# - 해결방안 : 무제한 서치를 통해 최대한 빠른 완성 후 다시 공부하기
# 24일 01시 30분
# - 목표재설정 :  최대한 빠른 완성
# - 사용 검색엔진 : 구글 및 ChatGPT4.0
# 24일 03시
# - ChatGPT4.0이 구글보다 훨씬 빠르고 상세하게 검색이 가능해서 ChatGPT4.0만을 이용하여 진행
# - 문제점 : 대부분의 필수 기능을 각각 구현 완료 했으나 통합하는 과정에서 대량의 오류 발생
# - 원인 : 능력 부족
# - 해결방안 : 무제한 ChatGPT 사용
# - 수면
# 24일 09시
# - ChatGPT4.0 구독서비스 결제완료
# - 각각 기능을 나누어져 있는 기능 통합하여 한 번에 완성 된 코드를 검색
# - 문제점 : 통합하여 검색했을 때 ChatGPT가 핵심기능은 인지을 하는데, 자세한 사항들은 검색된 코드에 반영이 안 된다.
# - 원인 : 검색을 할 때, 각각 기능들을 명확하게 구별하지 않고 한 문장에 섞어서 검색
# - 해결방안 : 핵심기능과 세부기능을 나누고 카테고리화 시킨 뒤, 카테고리에 맞게 기능을 단락단락 나누어 ChatGPT가 순차적으로 인식 가능하게 검색
# 24일 15시
# - 필수 기능 구현 완료
# - 도전 기능 구현 시작
# - ChatGPT에 기존의 코드를 보여주고 여기서 도전 기능을 추가한 코드를 검색
# - 문제점 : 저장 불러오기 기능 추가 후 대규모 에러 발생
# - 원인 : 프로젝트 안에 중복해서 들어가 있는 또 다른 Program.cs 
# - 해결방안 : 튜터님께 질문 뒤, 원인 파악 후 프로젝트에 있던 중복 cs 삭제
# 24일 21시
# - Struct/배열 형태로 아이템 정보를 관리하도록 변경시작
# - 문제점 : 코드 수정 후 대규모 에러 발생
# - 원인 : 파악불가
# - 해결방안 : ChatGPT사용하지 않고 스스로 나머지 도전 기능 구현
# 25일 01시
# - 문제점 : ChatGPT를 통해 구현 된 코드를 내가 유지/보수가 불가능
# - 원인 : 능력 부족
# - 해결방안 : ChatGPT에 각 코드 상세한 동작원리를 주석으로 달아달라고 요청
# 25일 04시
# - 문제점 : 능력 부족
# - 원인 : 자질 부족
# - 해결방안 : 개발자 과정 포기
# - 문제점 : 이상한 사고 흐름
# - 원인 : 피로
# - 해결방안 : 수면
# 25일 09시
# - 현재까지 작성 된 것 중 마지막의 정상적인 코드를 GitHub에 올리기
# - 문제점 : 만든 프로젝트 폴더가 GitHub Desktop에 올라가지 않음
# - 원인 : 그냥 폴더를 드래그 앤 드롭으로 GitHub Desktop 프로그램의 Repository에 옮김
# - 해결방안 : 우리조 팀장님께 질문
# 25일 14시
# - 프로젝트 최종 제출 및 회고
# - 이번 프로젝트에서 나는 개발자가 아니라 단순 입력 툴 및 유저인터페이스 디자이너였다.
# - 문제점 : 이 과제의 근본적인 제출의도에 부합하지 않는 방법으로 과제 수행
# - 원인 : 잘못된 목표 설정(기한 내 코드 완성)과 ChatGPT 무제한 사용으로 ChatGPT를 프로젝트 도구가 아닌 주체로써 개발자 포지션으로 이용
# - 해결방안 : 주말간 스스로 처음부터 다시 프로젝트 완성해보기
