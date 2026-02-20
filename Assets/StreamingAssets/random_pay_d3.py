import os
import random
import json
import sys


def get_random_video_data(directory):
    # 1. 디렉토리 내 mp4 파일 목록 추출
    if not os.path.exists(directory):
        return {"error": f"Directory not found: {directory}"}

    files = os.listdir(directory)
    mp4_files = [f for f in files if f.lower().endswith(".mp4")]

    if not mp4_files:
        return {"error": "No mp4 files found in the directory."}

    # 2. 랜덤하게 파일 하나 선택
    chosen_video = random.choice(mp4_files)
    full_path = os.path.abspath(os.path.join(directory, chosen_video))

    # 3. 원본 코드의 5개 세그먼트 정의
    # (start_time, end_time) 구조이며, -1은 영상 끝을 의미합니다.
    segments = [
        {"id": 1, "start": 0, "end": 10},
        {"id": 2, "start": 7, "end": 27},
        {"id": 3, "start": 14, "end": 34},
        {"id": 4, "start": 21, "end": -1},
        {"id": 5, "start": 28, "end": -1}
    ]

    # 4. JSON 데이터 구성
    result = {
        "video_path": full_path,
        "num_segments": len(segments),
        "segments": segments
    }
    return result


if __name__ == "__main__":
    # 유니티에서 인자로 경로를 넘겨줄 수 있도록 설정
    # 인자가 없으면 코드 내 기본 경로 사용
    default_dir = os.path.expanduser("./")
    target_dir = sys.argv[1] if len(sys.argv) > 1 else default_dir

    video_data = get_random_video_data(target_dir)

    # 유니티가 읽을 수 있도록 JSON 문자열로 출력
    print(json.dumps(video_data, ensure_ascii=False))