# Project Workflows - TalkFlow

## Phát triển tính năng mới
1. Tạo branch: `git checkout -b feature/<tên-tính-năng>`
2. Viết code trong các layer: Domain -> Application -> Infrastructure -> API
3. Commit: `git add . && git commit -m "Mô tả thay đổi"`
4. Push: `git push origin feature/<tên-tính-năng>`
5. Tạo Pull Request trên GitHub và merge vào `main`