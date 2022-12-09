// var headingNode = document.querySelector("html .box .heading-2");
// console.log(headingNode);
// var headingElement = document.querySelector(".heading");
// headingElement.innerHTML = "New heading";
// var boxElement = document.querySelector(".heading");
// boxElement.innerHTML = "<h1>Xin Chao</h1>";
// function log() {
//   console.log("Demo scope global");
// }
// function logg() {
//   log();
// }
// logg();
// {
//   const name = "nguyen ngoc son";
//   console.log(name);
// }
// function log() {
//   var name = "nguyen ngoc son";
//   console.log(name);
// }
// function create() {
//   let count = 0;
//   function increase() {
//     return ++count;
//   }
//   return increase;
// }
// const counter1 = create();
// console.log(counter1());
// console.log(counter1());
// console.log(counter1());
function createStorage(key) {
  const store = JSON.parse(localStorage.getItem(key)) ?? {};
  const save = () => {
    localStorage.setItem(key, JSON.stringify(store));
  };
  const storage = {
    get(key) {
      return store[key];
    },
    set(key, value) {
      store[key] = value;
      save();
    },
    remove(key) {
      delete store[key];
      save();
    },
  };
  return storage;
}

const setting = createStorage("settings");
console.log(setting.set("name"));
setting.set("name", "nguyen ngoc son");
