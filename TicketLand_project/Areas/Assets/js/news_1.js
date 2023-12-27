/*Ajax Xóa*/
$(document).ready(function () {
    $("#DeleteForm button[type='submit']").click(function () {
        // Lấy giá trị id cần xóa
        var form = $(this).closest('form');
        var newId = parseInt(form.closest('.modal').data('new-id'));
        console.log(newId);
        // Gửi Ajax request
        $.ajax({
            url: '/Admin/news/Delete/' + newId,
            type: 'POST',
            data: { id: newId },
            success: function (result) {
                // Xử lý kết quả sau khi xóa thành công
                console.log(result);

                // Đóng modal nếu xóa thành công
                $('#deleteForm').closest('.modal').modal('hide');
                window.location.reload();
            },
            error: function (error) {
                // Xử lý lỗi nếu có
                console.log(error);
            }
        });
    });
});
/*Kết thúc*/