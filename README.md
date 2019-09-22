# BE-A-DANCER-2

키넥트를 이용해서 사용자가 영상을 보고 춤을 따라하면 실시간으로 점수가 나오는 게임입니다. \
그리고 게임이 끝난 후 최종 점수와 사용자의 이름을 저장하고 랭킹을 볼 수 있습니다.


assets파일 : 
  1.	Animation : 환경 구현에 필요한 움직이는 애니메이션들
  2.	Guide : 가이드 관절 좌표 값. 사용자가 춤을 출 때 점수를 계산하기 위해서 가이드 관절 좌표 값이 필요함
  3.	Image : 환경 구현에 필요한 이미지들
  4.	Materials : 게임에 필요한 영상, 소리들
  5.	Pixel Font – Tripfive, Pixel UI : asset store에서 구매한 간단한 효과를 줄 수 있는 에셋들
  6.	Scenes : 게임 scene들
  7.	Scripts : 
		    BodySourceView : 사용자의 관절 좌표 값 뽑기, 프레임 뽑기, 점수 계산\
		    Change : 씬 이동에 필요\
		    Guidereader : 가이드 관절 좌표 값들을 gtxt로부터 읽어옴\
		    load_ranking : 랭킹을 보여주기 위해서 데이터 중 상위 5명의 사용자 이름과 점수를 불러옴\
		    manager : BodySourceView에서 계산한 점수 정보를 이용해서 점수 대역에 따라서 어떤 문구를 나가게 할지 결정\
		    nowscore : BodySourceView에서 계산한 점수를 디스플레이하기 위한 스크립트\
		    nowscore_hart : BodySourceView에서 계산한 점수 정보를 이용해서 에너지 바 구현\
		    scoremanager : 게임이 끝난 후 최종 점수 디스플레이\
	  	    song_id : 어떤 노래를 선택하느냐에 따라서 노래의 번호를 정해줘서 각 노래에 맞는 데이터를 이용할 수 있도록 함\
  		    viewpoint : 버튼이 클릭하는 시점부터 프레임을 비교해서 동기를 맞춤\
  
