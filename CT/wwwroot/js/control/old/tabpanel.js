/*
 * Grid
 * */
class TabPanel extends BaseControl {

    initControl() {
        var me = this;

        me.addEventToLinkTab();
    }

    addEventToLinkTab() {
        let me = this;
       
        
        $("#" + this.htmlID +' .mb-tablinks').on('click', function (event) {

            me.onTabClick(event);
        });
         
    }

    /*
     * Sự kiện nhấn vào tab
     * phamha
     * */
    onTabClick(evt) {
        var i, tabcontent, tablinks;
         
        tabcontent = $("#" + this.htmlID + " [mb_link_tab]");

        //ẩn hết Content
        for (i = 0; i < tabcontent.length; i++) {
            tabcontent[i].style.display = "none";
        }

        //Nút tab
        tablinks = $("#" + this.htmlID + " [mb_link_tab_to]");
        for (i = 0; i < tablinks.length; i++) {
            tablinks[i].className = tablinks[i].className.replace(" active", "");
        }

        //cái nào được nhấn thì cái đó active
        var targetID = evt.currentTarget.getAttribute('mb_link_tab_to');

        if (targetID) {

            $( '#' + this.htmlID + ' [mb_link_tab="' + targetID + '"]')[0].style.display = "block";
        }

        evt.currentTarget.className += " active";
    }

    selectIndex(index) {
        let me = this,
            i, tabcontent, tablinks;


        tabcontent = $("#" + this.htmlID + " [mb_link_tab]");

        //ẩn hết Content
        for (i = 0; i < tabcontent.length; i++) {
            tabcontent[i].style.display = "none";
        }

        //Nút tab
        tablinks = $("#" + this.htmlID + " [mb_link_tab_to]");
        for (i = 0; i < tablinks.length; i++) {
            tablinks[i].className = tablinks[i].className.replace(" active", "");
        }

        //cái nào được nhấn thì cái đó active
        //var targetID = evt.currentTarget.getAttribute('mb_link_tab_to');
        let tabLinkActive = $('.mb-tab .mb-tablinks')[index];
        let tabActive = $(tabLinkActive).attr('mb_link_tab_to');

        if (tabActive) {

            $('#' + this.htmlID + ' [mb_link_tab="' + tabActive + '"]')[0].style.display = "block";
        }

        $(tabLinkActive).addClass(" active");
    }

}