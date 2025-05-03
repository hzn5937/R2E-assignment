import React from 'react';
import { Pagination, Select } from 'antd';

const PaginationControls = ({ 
  pagination, 
  onPageChange, 
  onPageSizeChange,
  itemName = 'items' // default to 'items', can be overridden with 'books', 'requests', etc.
}) => {
  const { current, pageSize, total } = pagination;
  
  // Only show pagination if there are items
  if (total === 0) return null;
  
  return (
    <>
      {/* Row 1: Item count display and page size selection */}
      <div className="flex justify-between items-center mb-2 flex-wrap">
        <div>
          {/* Calculate the range manually */}
          {`${(current - 1) * pageSize + 1}-${Math.min(current * pageSize, total)} of ${total} ${itemName}`}
        </div>
        
        <div className="flex items-center">
          <span className="mr-2">Items per page:</span>
          <Select
            value={pageSize}
            onChange={onPageSizeChange}
            options={[
              { value: 5, label: '5' },
              { value: 10, label: '10' },
              { value: 25, label: '25' },
              { value: 50, label: '50' },
            ]}
            style={{ width: 80 }}
          />
        </div>
      </div>

      {/* Row 2: Just the pagination controls */}
      <div className="flex justify-center">
        <Pagination
          current={current}
          pageSize={pageSize}
          total={total}
          onChange={onPageChange}
          showSizeChanger={false}
          showQuickJumper
          showTotal={null}
        />
      </div>
    </>
  );
};

export default PaginationControls;