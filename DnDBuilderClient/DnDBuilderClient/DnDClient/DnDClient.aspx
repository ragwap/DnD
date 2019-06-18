<%@ Page Language="C#" %>
<!DOCTYPE html>
<html>
<head runat="server">
	<title>DnDClient</title>
    <script runat="server">
	
	</script>
    <script language="JavaScript" type="text/JavaScript">
            
            function viewAge(){
                var age = document.getElementById("age");
                var retAge = document.getElementById("retAge");
            
                retAge.innerHTML = age.value;
            
                age.oninput = function(){
                    retAge.innerHTML = this.value;
                }
                
            }
            
            function viewLevel(){
                var level = document.getElementById("level");
                var retLevel = document.getElementById("retLevel");
            
                retLevel.innerHTML = level.value;
            
                level.oninput = function(){
                    retLevel.innerHTML = this.value;
                }
                isSpellCaster();
            }
            
            function getRaceList(){
                race = new XMLHttpRequest();
                var raceDest = "/Race/List";
            
                race.open("GET", raceDest, true);
                race.onreadystatechange = function() {
            
                    if(race.readyState == 4) {
            
                        if(race.status == 200) {
            
                            var raceClient = document.getElementById("race");
            
                            raceRetVal = JSON.parse(race.responseText);   
            
                            for(var raceKey in raceRetVal){
            
                                raceClient.options[raceKey] = new Option(raceRetVal[raceKey]);
                            }
                        }
                    }
                };
                race.send();
            }
            
            function getClassList(){
                classList = new XMLHttpRequest();
                var classDest = "/Class/List";
            
                classList.open("GET", classDest, true);
                classList.onreadystatechange = function() {
            
                    if(classList.readyState == 4) {
            
                        if(classList.status == 200) {
            
                            var classClient = document.getElementById("class");
            
                            classRetVal = JSON.parse(classList.responseText);   
            
                            for(var classKey in classRetVal){
            
                                classClient.options[classKey] = new Option(classRetVal[classKey]);
            
                            }
                        }
                    }
                };
                classList.send();
            }
            
            
            function isSpellCaster(){
                spell = new XMLHttpRequest();
            
                var cClient = document.getElementById("class").value;

                for(var cKey in classRetVal){
                    if(classRetVal[cKey] == cClient){
                        
                        var spellDest = "/Class/List/" + cKey;
            
                        spell.open("GET", spellDest, true);
            
                        spell.onreadystatechange = function(){
                            if(spell.readyState == 4){
                                if(spell.status == 200){
                                    var spellRetVal = JSON.parse(spell.responseText);
            
                                    for(var ret in spellRetVal)
                                    {
                                        if (cClient != null){
                                            
                                            if(ret == "spell"){
                                                document.getElementById("can").checked = true;
                                                document.getElementById("cannot").checked = false;
                                            }
                                            else {
                                                document.getElementById("cannot").checked = true;
                                                document.getElementById("can").checked = false;
                                            }
            
                                            var constitution = document.getElementById("constitution").value;

                                            hitDie = spellRetVal[ret];
                                            
                                            document.getElementById("hitDie").value = spellRetVal[ret];
                                            document.getElementById("hitPoints").innerHTML = (document.getElementById("level").value * hitDie) + parseInt(constitution, 10);
                                        }
            
                                        else{
                                            
                                            document.getElementById("can").checked = false;
                                            document.getElementById("cannot").checked = false;
                                        }
                                    }   
                           console.log(document.getElementById("can").value);             
                                }
                           }
                        };
                        spell.send();
                    }
                }   
            
            }
            
             function getAbilityScores(){
                ability = new XMLHttpRequest();
            
                var rClient = document.getElementById("race").value;
            
                for(var rKey in raceRetVal){
            
                    if(rClient == raceRetVal[rKey]){
            
                        var abilityDest = "/Race/List/" + rKey;
            
                        ability.open("GET", abilityDest, true);
            
                        ability.onreadystatechange = function(){
                            
                            if(ability.readyState == 4){

                                if(ability.status == 200){
                                
                                    abilityRetVal = JSON.parse(ability.responseText);
            
                                    var constitution = abilityRetVal[0];
                                    dexterity = abilityRetVal[1];
                                    strength = abilityRetVal[2];
                                    charisma = abilityRetVal[3];
                                    intelligence = abilityRetVal[4];
                                    wisdom = abilityRetVal[5];
            
                                    document.getElementById("constitution").value = abilityRetVal[0];
            
                                    var hitDie = document.getElementById("hitDie").value;
            
                                    document.getElementById("hitPoints").innerHTML = (document.getElementById("level").value * parseInt(hitDie, 10)) + constitution;

                                }
                            }
                        };
            ability.send();
                    }
                }
                
            }
            
            function dbCon(){
                con = new XMLHttpRequest();
            
                var conDest = "/DB/Connection";
            
                con.open("GET", conDest, true);
            
                con.send();
            }
            
            function dbInsert(){
            
                var name = document.getElementById("cname").value;
                var age = document.getElementById("age").value;
                var gender = document.getElementById("gender").value;
                var bio = document.getElementById("bio").value;
                var level = document.getElementById("level").value;
                var race = document.getElementById("race").value;
                var cClass = document.getElementById("class").value;
                var spell = "no";
                var hit = document.getElementById("hitPoints").innerHTML;
                var abilityScore = document.getElementById("ability").innerHTML;
            
                if(document.getElementById("can").checked){
                
                    spell = "yes";
                }
            
                if(name != ""){
            
                    insert = new XMLHttpRequest();
           
                    if(gender == ""){
                        gender = null;
                    }
            
                    if(bio == ""){
                        bio = null;
                    }
            
                    if(cClass == ""){
                        cClass = null;
                    }
            
                    if(race == ""){
                        race = null;
                    }
                  
                    var insertDest = "/Insert/" + name + "/" + age + "/" + gender + "/" + bio + "/" + level + "/" + race + "/" + cClass + "/" + spell + "/" + parseInt(hit, 10) + "/" + parseInt(abilityScore, 10);
            
                    insert.open("GET", insertDest, true);
                
                    insert.onreadystatechange = function(){
                
                        if(insert.readyState == 4){
            
                            if(insert.status == 200){
                
                                var ret = JSON.parse(insert.responseText);
            
                                if(ret != null){
                                    alert(ret);
                                    init();
                                }
                
                            }
                        }
                    }
                    insert.send();
                }
            
                else{
                    alert("Name is a required field and should be unique");
                }
            }
            
            function dbDelete(){
            
                if(confirm("Are you sure you want to Delete?")){
                    del = new XMLHttpRequest();
            
                    var name = document.getElementById("cname").value;
                
                    var deleteDest = "/Delete/" + name;
                
                    del.open("GET", deleteDest, true);
                
                    del.onreadystatechange = function(){
                
                        if(del.readyState == 4){
                
                            if(del.status == 200){
                
                                var delMsg = JSON.parse(del.responseText);
                
                                if(delMsg != null){
                                    alert(delMsg);
                                    init();
                                }
                            }
                        }
                    }
                    del.send();
                }
                
            }
            
            function dbUpdate(){
            
                if(confirm("Are you sure you want to edit?")){
            
                    var name = document.getElementById("cname").value;
                    var age = document.getElementById("age").value;
                    var gender = document.getElementById("gender").value;
                    var bio = document.getElementById("bio").value;
                    var level = document.getElementById("level").value;
                    var race = document.getElementById("race").value;
                    var cClass = document.getElementById("class").value;
                    var spell = "no";
                    var hit = document.getElementById("hitPoints").innerHTML;
                    var abilityScore = document.getElementById("ability").innerHTML;
                
                    if(document.getElementById("can").checked){
                    
                        spell = "yes";
                    }
                
                    if(name != ""){
                        update = new XMLHttpRequest();
            
                        if(gender == ""){
                            gender = null;
                        }
                
                        if(bio == ""){
                            bio = null;
                        }
                
                        if(cClass == ""){
                            cClass = null;
                        }
                
                        if(race == ""){
                            race = null;
                        }

                        var updateDest = "/Update/" + name + "/" + age + "/" + gender + "/" + bio + "/" + level + "/" + race + "/" + cClass + "/" + spell + "/" + parseInt(hit, 10) + "/" + parseInt(abilityScore, 10);
            
                        update.open("GET", updateDest, true);
            
                        update.onreadystatechange = function(){
            
                            if(update.readyState == 4){
            console.log(update.status);
                                if(update.status == 200){
                                    var ret = JSON.parse(update.responseText);
            
                                    if(ret != null){
                                        alert(ret);
                                    }
                                }
                            }
                        }
                        update.send();
                    }
                }
                
            }
            
            function viewAll(){
                vAll = new XMLHttpRequest();
            
                var allPath = "/All/List";
            
                vAll.open("GET", allPath, true);
            
                vAll.onreadystatechange = function(){
                    if(vAll.readyState == 4){
            
                        if(vAll.status == 200){
            console.log(vAll.status);
                            var ret = JSON.parse(vAll.responseText);
            
                            var lAll = document.createElement("TABLE");
                            lAll.setAttribute("id", "dndAll");
                            document.body.appendChild(lAll);
                            
                                for(var item in ret){
            
                                    var tr1 = document.createElement("TR");
                                    tr1.setAttribute("id", "headLevel");
                                    document.getElementById("dndAll").appendChild(tr1);
                        
                                    var th1 = document.createElement("TH");
                                    var header1 = document.createTextNode(item);
                                    th1.appendChild(header1);
                                    document.getElementById("headLevel").appendChild(th1);
            
                                    var th2 = document.createElement("TH");
                                    var header2 = document.createTextNode("");
                                    th2.appendChild(header2);
                                    document.getElementById("headLevel").appendChild(th2);
                
                                    var i = 0;
            
                                    for(var val in ret[item]){
                                        var tr2 = document.createElement("TR");
                                        tr2.setAttribute("id", i.toString());
                                        document.getElementById("dndAll").appendChild(tr2);
                            
                                        var td1 = document.createElement("TD");
                                        var data1 = document.createTextNode(ret[item][val]);
                                        td1.appendChild(data1);
                                        document.getElementById(i.toString()).appendChild(td1);
                    
                                        var td2 = document.createElement("TD");
                                        var data2 = document.createTextNode("");
                                        td2.appendChild(data2);
                                        document.getElementById(i.toString()).appendChild(td2);
                    
                                        ++i;
                                    }
            
                                }
                            
                            /*else(
                                alert("No Records to be displayed");
                            }*/
                        }
                    }
                }
                vAll.send();
            }
            
            function singleCharacter(){
            
                var name = document.getElementById("cname").value;
                document.getElementById("tempName").value = name;
            
                vSingle = new XMLHttpRequest();
            
                var singlePath = "/View/Character/" + name;
            
                vSingle.open("GET", singlePath, true);
            
                vSingle.onreadystatechange = function(){
                    if(vSingle.readyState == 4){
                        if(vSingle.status == 200){
                            var ret = JSON.parse(vSingle.responseText);
                            console.log(ret);
            
                            var singleList = document.createElement("TABLE");
                            singleList.setAttribute("id", "dndTbl");
                            document.body.appendChild(singleList);
                            
                            for(res in ret){
            
                                if(res == "error"){
                                    alert(ret[res]);
                                }
                        
                                else{
                                    var tr = document.createElement("TR");
                                    tr.setAttribute("id", res);
                                    document.getElementById("dndTbl").appendChild(tr);
                        
                                    var td1 = document.createElement("TD");
                                    var data1 = document.createTextNode(res);
                                    td1.appendChild(data1);
                                    document.getElementById(res).appendChild(td1);
                
                                    var td2 = document.createElement("TD");
                                    var data2 = document.createTextNode(ret[res]);
                                    td2.appendChild(data2);
                                    document.getElementById(res).appendChild(td2);
                                }
                                
                            }
            
                            document.getElementById("download").style.visibility = "visible";
          
                        }
                    }
                }
                vSingle.send();
            }
            
            function downloadCharacter(){
                download = new XMLHttpRequest();
            
                var tempName = document.getElementById("tempName").value;
                var destDownload = "/Download/Character/" + tempName;
            
                download.open("GET", destDownload, true);
            
                download.onreadystatechange = function(){
                    if(download.readyState == 4){
                        if(download.status == 200){
                            var res = JSON.parse(download.responseText);
            
                            if(res != null){
                                alert(res);
                            }
                        }
                    }
                }   
                download.send();
            }
            
            function init(){
                
                var classRetVal;
                var raceRetVal;
                var abilityRetVal;
                var HitPoints;
                var hitDie;
                //var constitution = 0;
                var dexterity = 0;
                var strength = 0;
                var charisma = 0;
                var intelligence = 0;
                var wisdom = 0;
                document.getElementById("can").checked = false;
                document.getElementById("cannot").checked = false;
                document.getElementById("level").value = 1;
                document.getElementById("retLevel").innerHTML = 1
                document.getElementById("age").value = 0;
                document.getElementById("retAge").innerHTML = 0;
                document.getElementById("cname").value = null;
                document.getElementById("gender").value = null;
                document.getElementById("bio").value = null;
                document.getElementById("class").vlaue = null;
                document.getElementById("race").value = null;
                document.getElementById("hitPoints").innerHTML = 0;
                document.getElementById("ability").innerHTML = 0;
                document.getElementById("constitution").value = 0;
                document.getElementById("hitDie").value = 0;
                document.getElementById("download").style.visibility = "hidden";
                document.getElementById("tempName").value = null;
            
                getClassList();
                getRaceList();
                dbCon();
                
            }
            
            
            
    </script>
</head>
<body onload="init()">
        <div>
            <center><h1>DnD Character Customization</h1></center>
        </div>
        <div>
            <center>
                <form id="form1" runat="server" style="margin-top:10%">
                    <table>
                        <tr>
                            <td>
                                <label for = "cname">Name</label>
                            </td>
                            <td>
                                <input type = "text" id = "cname" placeholder = "Enter character Name" required/><br>
                            </td>
                        </tr>
                        <tr style="margin-top:2%">
                            <td>
                                <label for = "age">Age</label>
                            </td>
                            <td>
                                <input type = "range" min = "0" max = "500" id = "age" onchange="viewAge()"/>
                                <p id="retAge"></p>
                            </td>
                        </tr>
                        <tr style="margin-top:2%">
                            <td>
                                <label for = "gender">Gender</label>
                            </td>
                            <td>
                                <input type = "text" id = "gender" placeholder = "Enter character Gender"/><br>
                            </td>
                        </tr>
                        <tr style="margin-top:2%">
                            <td>
                                <label for = "bio">Biography</label>
                            </td>
                            <td>
                                <textarea id="bio"></textarea>
                                
                            </td>
                        </tr>
                        <tr style="margin-top:2%">
                            <td>
                                <label for = "level">Level</label>
                            </td>
                            <td>
                                <input type = "range" min = "1" max = "20" id = "level" onchange="viewLevel()"/><br>
                                <p id="retLevel"></p>
                            </td>
                        </tr>
                        <tr style="margin-top:2%">
                            <td>
                                <label for = "race">Race</label>
                            </td>
                            <td>
                                <select id = "race" onchange="getAbilityScores()"></select><br>
                            </td>
                        </tr>
                        <tr style="margin-top:2%">
                            <td>
                                <label for = "class">Class</label>
                            </td>
                            <td>
                                <select id = "class" onchange="isSpellCaster()"></select><br>
                            </td>
                        </tr>
                        <tr style="margin-top:2%" rowspan="2">
                            <td>
                                Spellcaster
                            </td>
                            <td>
                                <input id="can" type = "radio" name = "spell" value = "yes" disabled="disabled"/>Can cast spells<br>
                                <input id="cannot" type = "radio" name = "spell" value = "no" disabled="disabled"/>Cannot cast spells<br>
                            </td>
                        </tr>
                        <tr style="margin-top:2%">
                            <td>
                                Hit Points
                            </td>
                            <td>
                                <p id="hitPoints"></p>
                            </td>
                        </tr>
                        <tr style="margin-top:2%">
                            <td>
                                Ability Scores
                            </td>
                            <td>
                                <p id="ability"></p>
                            </td>
                        </tr>
                    </table>
                    
                    <input type="hidden" id="constitution">
                    <input type="hidden" id="hitDie">
                    <input type="hidden" id="tempName">
                    
                    <div style="margin-top:5%">
                        <input type="button" value="Create" id="create" onclick="dbInsert()">
                        <input type="button" value="View Character List" id="viewList" onclick="viewAll()">
                        <input type="button" value="View Character" id="viewCharacter" onclick="singleCharacter()">
                        <input type="button" value="Edit" id="edit" onclick="dbUpdate()">
                        <input type="button" value="Delete" id="delete" onclick="dbDelete()">
                        <input type="button" value="Download" id="download" onclick="downloadCharacter()">
                    </div>
                    <br>
                    <br>
                    
                </form>
            </center>
        </div> 
    </body>
</html>
