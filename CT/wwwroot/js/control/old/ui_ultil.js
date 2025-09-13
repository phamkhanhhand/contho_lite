/*
 * Lớp gán Fit screen
 * phamkhanhhand
 * */
class FitScreen {
    objectMap = null

    constructor(objectMap) {
        this.objectMap = objectMap;
    }

    setLayout() {
        var me = this;

        me.setFitScreen(me.objectMap);
    }



    getRootSize() {

    }

    /*
     * Hàm đặt objectMap vào full screen
     * Quy hết về pixcel
     * Chú ý: width, height ở đây là chứa cả padding, margin, border
     * */
    setFitScreen(objectMap) {
        var me = this;


        //Nếu có hàm thì phải lấy hàm
        if (objectMap && objectMap.fitItSelf && App[objectMap.id] && App[objectMap.id].getSizeObject)
        {

            //xem xét, chưa chắc đã cần
            //So với bên ngoài
            let top = objectMap.top;

            let bottom = objectMap.bottom;
            let left = objectMap.left;
            let right = objectMap.right;
             
            //#region Nội dung bên trong thẻ
            let height = objectMap.height;
            let width = objectMap.width;

            let paddingTop = objectMap.paddingTop;
            let paddingBottom = objectMap.paddingBottom;
            let paddingLeft = objectMap.paddingLeft;
            let paddingRight = objectMap.paddingRight;

            let marginTop = objectMap.marginTop;
            let marginBottom = objectMap.marginBottom;
            let marginLeft = objectMap.marginLeft;
            let marginRight = objectMap.marginRight;

            let borderTop = objectMap.borderTop;
            let borderBottom = objectMap.borderBottom;
            let borderLeft = objectMap.borderLeft;
            let borderRight = objectMap.borderRight;

            //#endregion

            let id = objectMap.id;
            let fitScreen = objectMap.fitScreen;

            objectMap = App[objectMap.id].getSizeObject();
            //Đặt lại cho nó
            objectMap.top = top;
            objectMap.left = left;
            objectMap.bottom = bottom;
            objectMap.id = id;
            objectMap.height = height;
            objectMap.width = width;
            objectMap.fitScreen = fitScreen;
             
        }

        //phamkhanhhand
        //Quy đổi root (tạm để lại)
        //Quy đổi phần trăm ra số
        //Quy đổi Bổ sung cái nào còn thiếu thì lấy luôn trên thẻ
        //tính toán

        if (objectMap && objectMap.id) {
            var curContainer = $("#" + objectMap.id);
             
            //Nếu là cái đầu tiên (lấy từ màn hình screen)
                //sau sẽ tách thành 1 hàm
            if (objectMap.fitScreen) {
                var screenHeight = $(window).height();
                var screenWidth= $(window).width();
                //Nếu chưa có chiều cao thì cho bằng chiều cao màn hình luôn 
                if (!objectMap.height) {
                    objectMap.height = screenHeight - 38;//Theo trang master
                }
                if (!objectMap.width) {
                    objectMap.width = screenWidth - 200;//cái này theo trang master
                } 
            } 

            //#region nếu null thì lấy ở luôn trên thẻ (màn hình nhé)
            if (!objectMap.height) {
                objectMap.height = parseInt(curContainer.css('height'));
            }
            if (!objectMap.width) {
                objectMap.width = parseInt(curContainer.css('width'));
            }
            if (!objectMap.marginTop) {
                objectMap.marginTop = parseInt(curContainer.css('margin-top'));
            }
            if (!objectMap.marginBottom) {
                objectMap.marginBottom = parseInt(curContainer.css('margin-bottom'));
            }
            if (!objectMap.marginLeft) {
                objectMap.marginLeft = parseInt(curContainer.css('margin-left'));
            }
            if (!objectMap.marginRight) {
                objectMap.marginRight = parseInt(curContainer.css('margin-right'));
            }

            if (!objectMap.paddingTop) {
                objectMap.paddingTop = parseInt(curContainer.css('padding-top'));
            }
            if (!objectMap.paddingBottom) {
                objectMap.paddingBottom = parseInt(curContainer.css('padding-bottom'));
            }
            if (!objectMap.paddingLeft) {
                objectMap.paddingLeft = parseInt(curContainer.css('padding-left'));
            }
            if (!objectMap.paddingRight) {
                objectMap.paddingRight = parseInt(curContainer.css('padding-right'));
            }
             
            if (!objectMap.borderTop) {
                objectMap.borderTop = parseInt(curContainer.css('border-top-width'));
            }
            if (!objectMap.borderBottom) {
                objectMap.borderBottom = parseInt(curContainer.css('border-bottom-width'));
            }
            if (!objectMap.borderLeft) {
                objectMap.borderLeft = parseInt(curContainer.css('border-left-width'));
            }
            if (!objectMap.borderRight) {
                objectMap.borderRight = parseInt(curContainer.css('border-right-width'));
            }
             
            //#endregion 


            //Đặt chiều cao và chiều rộng đã trừ đi
            curContainer.width(objectMap.width - objectMap.marginLeft - objectMap.borderLeft - objectMap.paddingLeft
                - objectMap.marginRight - objectMap.borderRight - objectMap.paddingRight);

            curContainer.height(objectMap.height - objectMap.marginTop- objectMap.borderBottom - objectMap.paddingTop
                - objectMap.marginBottom- objectMap.borderTop - objectMap.paddingBottom);

             

            //mấy cái kia tự đặt(todo đặt để override)


            if (objectMap.overflow) {
                curContainer.css({
                    overflow: 'hidden',
                });
            }
            else {
                curContainer.css({
                    overflow: 'auto',
                });
            }

            if (objectMap.hasHorizontalChild) {
                 
                curContainer.css({
                    'display': 'flex',
                });

            }

            //curContainer.attr('overflow-x', 'auto');

            //TODO phân cho con các chiều

            var lstChild = objectMap.children;
            var parentHeight = curContainer.height();
            //var parentWidth= curContainer.width();

            var parentWidth = objectMap.width;
             
            if (!parentWidth) {
              parentWidth= curContainer.width(); 
            }


            if (lstChild) {

                var flexChild = [];
                //chiều cao đã dùng cho flex
                var fixHeight = 0;

                for (var i = 0; i < lstChild.length; i++) {
                    var child = lstChild[i];

                    //Nếu là % thì cứ quy đổi ra px thôi
                    if (child.height && child.height.toString().endsWith("%")) {
                        child.height = (child.height.replace('%', '')) * parentHeight * 0.01;
                    }
                    if (child.width && child.width.toString().endsWith("%")) {
                         
                        child.width = (child.width.replace('%', '')) * parentWidth * 0.01;
                    }
                    //Nếu là tỷ lệ phần trăm thì nhân nó vào đây
                    //Nếu là bình thường thì đặt thẳng
                    //Lưu lại danh sách các thằng flex để đặt cho nó

                    //Nếu k có width thì cho nó = 100% cha
                    if (!child.width) {
                        child.width = parentWidth;
                    }

                    //Nếu k có chiều cao
                    if (!child.height) {
                        flexChild.push(child); 
                    }
                    else {

                        me.setFitScreen(child);
                        //dấu cộng không phải là thừa đâu nhé, mà là để quy đổi chữ ra số
                        fixHeight += +child.height + (+child.border|0);
                        //fixHeight += +child.height;
                    }
                }//for


                //bắt đầu chia cho các cái flex còn lại
                if (flexChild && flexChild.length > 0) {
                    var leftEach = (parentHeight - fixHeight) / flexChild.length;
                      
                    for (var j = 0; j < flexChild.length; j++) {
                        flexChild[j].height = leftEach;
                        me.setFitScreen(flexChild[j]);
                    }

                }

            }

        }
    }


}


/*
 * Đổi id để nhỡ ra nó có gọi lại ở nhiều màn hình thì không chớt
 * phamkhanhhand
 * */
function changeUIID() {
    var idx = "_" + PKHA.commonFn.uuidv4();
    var lstDynamicTagID = $('.mbfit');

    for (var i = 0; i < lstDynamicTagID.length; i++) {
        var currTag = $(lstDynamicTagID[i]);

        var id = currTag.attr('id')+idx;
        currTag.attr('id', id);

    }
}