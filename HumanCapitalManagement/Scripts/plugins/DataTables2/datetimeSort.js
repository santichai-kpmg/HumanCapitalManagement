// ฟังก์ชั่น เติม 0 ด้านหน้า    อันนี้ไม่ได้ใช้ เก็บไว้เผื่อเป็นประโยชน์
var smName = {
    Jan: 1,
    Feb: 2,
    Mar: 3,
    Apr: 4,
    May: 5,
    Jun: 6,
    Jul: 7,
    Aug: 8,
    Sep: 9,
    Oct: 10,
    Nov: 11,
    Dec: 12
};

function padDigits(number, digits) {
    return Array(Math.max(digits - String(number).length + 1, 0)).join(0) + number;
}
// สร้าง plugin สำหรับวันที่ภาษาไทยของเรา    ในที่นี้กำหนดใช้ชื่อ date-th ไว้อ้างอิง
jQuery.extend(jQuery.fn.dataTableExt.oSort, {
    "date-th-pre": function (a) {
        var x;
        if ($.trim(a) !== '') {
            // a คือข้อความวันที่ใน column ของแต่ละแถว
            // ส่วนนี้จะเป็นส่วนแยก เอาค่าต่างๆ ไปใช้สร้างวันที่

            var dateData = $.trim(a).split(" ");
            var d_date = dateData[0].split("-");
            var t_date = dateData[1];
            var h_time = $.trim(t_date).substring(1, 2);
            var s_time = $.trim(t_date).substring(4, 2);

            var sh = $.trim(t_date).substring(0, 2);
            var sm = $.trim(t_date).substring(3, 5);
            var ss = $.trim(t_date).substring(6, 8);


            var yearVal = d_date[2];// - 543;
            // จบการแยกค่าต่างๆ ออกจากข้อความ
            var myDate = new Date(yearVal, ShortMnameToNumber(d_date[1]), d_date[0], sh, sm, ss, 000);
            // เราจะเก็บวันที่ที่ถูกแปลงเป็นตัวเลขด้วย myDate.getTime() ไว้ในตัวแปร x
            // ไว้สำหรับเทียบค่ามากกว่า น้อยกว่า
            x = (myDate.getTime()) * 1;

        } else {
            // ภ้าช่องนั้นมีค่าเป็นว่าง กำหนดเป็น x เป็น Infinity
            x = Infinity;
        }
        // คืนค่ารูปแบบวันที่ที่ถูกแปลงเป็นตัวเลข เพื่อนำไปจัดเรียง
        return x;
    },
    "date-th-asc": function (a, b) { // กรณีให้เรียงจากน้อยไปมาก
        return a - b;
    },
    "date-th-desc": function (a, b) { // กรณีให้เรียงจากมากไปน้อย
        return b - a;
    }
});
jQuery.extend(jQuery.fn.dataTableExt.oSort, {
    "date-th2-pre": function (a) {
        var x;
        if ($.trim(a) !== '') {
            // a คือข้อความวันที่ใน column ของแต่ละแถว
            // ส่วนนี้จะเป็นส่วนแยก เอาค่าต่างๆ ไปใช้สร้างวันที่
            var dateData = $.trim(a);
            var d_date = dateData.split("-");
            //var yearVal = d_date[2] - 543;
            var yearVal = d_date[2];
            // จบการแยกค่าต่างๆ ออกจากข้อความ
            var myDate = new Date(yearVal, ShortMnameToNumber(d_date[1]), d_date[0], 0, 0, 00, 000);
            // เราจะเก็บวันที่ที่ถูกแปลงเป็นตัวเลขด้วย myDate.getTime() ไว้ในตัวแปร x
            // ไว้สำหรับเทียบค่ามากกว่า น้อยกว่า
            x = (myDate.getTime()) * 1;

        } else {
            // ภ้าช่องนั้นมีค่าเป็นว่าง กำหนดเป็น x เป็น Infinity
            x = Infinity;
        }
        // คืนค่ารูปแบบวันที่ที่ถูกแปลงเป็นตัวเลข เพื่อนำไปจัดเรียง
        return x;
    },
    "date-th2-asc": function (a, b) { // กรณีให้เรียงจากน้อยไปมาก
        return a - b;
    },
    "date-th2-desc": function (a, b) { // กรณีให้เรียงจากมากไปน้อย
        return b - a;
    }
});

function ShortMnameToNumber(val) {
    var nReturn = 0;
    if (smName[val] != undefined) {
        nReturn = smName[val]
    }
    return nReturn;
}