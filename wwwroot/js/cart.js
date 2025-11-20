// === CART MANAGER ===
// Lưu & cập nhật giỏ hàng trong localStorage
const CartManager = {
  key: "gioHang",

  getAll() {
    return JSON.parse(localStorage.getItem(this.key)) || [];
  },

  save(data) {
    localStorage.setItem(this.key, JSON.stringify(data));
    this.updateBadge();
  },

  addProduct(product) {
    let cart = this.getAll();
    let index = cart.findIndex((p) => p.sanpham_id === product.sanpham_id);

    if (index > -1) {
      cart[index].so_luong += product.so_luong;
    } else {
      cart.push(product);
    }

    this.save(cart);
    this.showToast("🛒 Đã thêm vào giỏ hàng!");
  },

  removeProduct(id) {
    let cart = this.getAll().filter((p) => p.sanpham_id !== id);
    this.save(cart);
  },

  countItems() {
    return this.getAll().reduce((sum, p) => sum + p.so_luong, 0);
  },

  updateBadge() {
    const badge = document.getElementById("cartCount");
    if (badge) badge.innerText = this.countItems();
  },

  showToast(message) {
    const container =
      document.getElementById("toastContainer") ||
      (() => {
        const div = document.createElement("div");
        div.id = "toastContainer";
        div.className = "toast-container position-fixed top-0 end-0 p-3";
        div.style.zIndex = "1100";
        document.body.appendChild(div);
        return div;
      })();

    const toast = document.createElement("div");
    toast.className =
      "toast align-items-center text-white bg-success border-0 show";
    toast.role = "alert";
    toast.innerHTML = `
            <div class="d-flex">
                <div class="toast-body">${message}</div>
                <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button>
            </div>`;
    container.appendChild(toast);

    setTimeout(() => {
      toast.classList.remove("show");
      setTimeout(() => toast.remove(), 500);
    }, 2000);
  },
};

// Khởi động khi load trang
document.addEventListener("DOMContentLoaded", () => {
  CartManager.updateBadge();
});
