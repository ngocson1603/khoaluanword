from django.contrib import admin
from .models import employees
# Register your models here.


class EmployeeAdmin(admin.ModelAdmin):
    list_display = ('employee_id', 'department_id',
                    'name', 'age', 'avatar', 'cv')
    search_fields = ['name']
    list_filter = ('employee_id', 'department_id',
                   'name', 'age', 'avatar', 'cv')


admin.site.register(employees, EmployeeAdmin)
