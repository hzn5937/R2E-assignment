import React from 'react';
import { Select, Button, Space, Divider } from 'antd';
import { FilterOutlined, ClearOutlined } from '@ant-design/icons';

const FilterBar = ({ 
  categories, 
  selectedCategory, 
  availabilityFilter, 
  onCategoryChange, 
  onAvailabilityChange, 
  onApplyFilters, 
  onClearFilters,
  isFilterApplied,
  filterLoading
}) => {
  return (
    <div className="mb-4 p-4 border border-gray-200 rounded bg-gray-50">
      <div className="flex flex-wrap gap-4 items-center">
        <div>
          <label className="block text-sm font-medium mb-1">Category</label>
          <Select
            placeholder="Select Category"
            style={{ width: 200 }}
            value={selectedCategory}
            onChange={onCategoryChange}
            allowClear
            options={[
              { value: null, label: 'All Categories' },
              ...categories.map(category => ({ 
                value: category.id, 
                label: category.name 
              }))
            ]}
          />
        </div>
        
        <div>
          <label className="block text-sm font-medium mb-1">Availability</label>
          <Select
            placeholder="Filter by Availability"
            style={{ width: 200 }}
            value={availabilityFilter}
            onChange={onAvailabilityChange}
            allowClear
            options={[
              { value: null, label: 'All Books' },
              { value: true, label: 'Available Books' },
              { value: false, label: 'Unavailable Books' }
            ]}
          />
        </div>
        
        <div className="mt-6">
          <Space>
            <Button 
              type="primary" 
              icon={<FilterOutlined />} 
              onClick={onApplyFilters}
              loading={filterLoading}
            >
              Apply Filters
            </Button>
            
            {isFilterApplied && (
              <Button 
                icon={<ClearOutlined />} 
                onClick={onClearFilters}
              >
                Clear Filters
              </Button>
            )}
          </Space>
        </div>
      </div>
    </div>
  );
};

export default FilterBar;