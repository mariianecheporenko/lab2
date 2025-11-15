<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    
    <xsl:output method="html" encoding="UTF-8" indent="yes"/>
    
    <xsl:template match="/">
        <html>
            <head>
                <meta charset="UTF-8"/>
                <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
                <title>Гуртожиток КНУ - База мешканців</title>
                <style>
                    * {
                        margin: 0;
                        padding: 0;
                        box-sizing: border-box;
                    }
                    
                    body {
                        font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                        background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
                        min-height: 100vh;
                        padding: 20px;
                    }
                    
                    .container {
                        max-width: 1400px;
                        margin: 0 auto;
                        background: white;
                        border-radius: 15px;
                        box-shadow: 0 20px 60px rgba(0, 0, 0, 0.3);
                        overflow: hidden;
                    }
                    
                    header {
                        background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
                        color: white;
                        padding: 40px;
                        text-align: center;
                    }
                    
                    header h1 {
                        font-size: 2.5em;
                        margin-bottom: 10px;
                        text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.2);
                    }
                    
                    header p {
                        font-size: 1.2em;
                        opacity: 0.9;
                    }
                    
                    .stats {
                        display: grid;
                        grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
                        gap: 20px;
                        padding: 30px;
                        background: #f8f9fa;
                        border-bottom: 2px solid #e9ecef;
                    }
                    
                    .stat-card {
                        background: white;
                        padding: 20px;
                        border-radius: 10px;
                        text-align: center;
                        box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
                        transition: transform 0.3s;
                    }
                    
                    .stat-card:hover {
                        transform: translateY(-5px);
                    }
                    
                    .stat-card .number {
                        font-size: 2.5em;
                        font-weight: bold;
                        color: #667eea;
                    }
                    
                    .stat-card .label {
                        color: #6c757d;
                        margin-top: 5px;
                        font-size: 0.9em;
                    }
                    
                    .content {
                        padding: 30px;
                    }
                    
                    .table-wrapper {
                        overflow-x: auto;
                        margin-top: 20px;
                    }
                    
                    table {
                        width: 100%;
                        border-collapse: collapse;
                        background: white;
                    }
                    
                    thead {
                        background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
                        color: white;
                    }
                    
                    th {
                        padding: 15px;
                        text-align: left;
                        font-weight: 600;
                        text-transform: uppercase;
                        font-size: 0.85em;
                        letter-spacing: 0.5px;
                    }
                    
                    td {
                        padding: 15px;
                        border-bottom: 1px solid #e9ecef;
                    }
                    
                    tbody tr {
                        transition: background-color 0.3s;
                    }
                    
                    tbody tr:hover {
                        background-color: #f8f9fa;
                    }
                    
                    tbody tr:nth-child(even) {
                        background-color: #fafbfc;
                    }
                    
                    tbody tr:nth-child(even):hover {
                        background-color: #f1f3f5;
                    }
                    
                    .badge {
                        display: inline-block;
                        padding: 4px 10px;
                        border-radius: 12px;
                        font-size: 0.8em;
                        font-weight: 600;
                        margin: 2px;
                    }
                    
                    .badge-scholarship {
                        background-color: #d4edda;
                        color: #155724;
                    }
                    
                    .badge-benefits {
                        background-color: #fff3cd;
                        color: #856404;
                    }
                    
                    .badge-course {
                        background-color: #e7f3ff;
                        color: #004085;
                    }
                    
                    .faculty-tag {
                        font-size: 0.85em;
                        color: #6c757d;
                        font-style: italic;
                    }
                    
                    .room-number {
                        font-weight: bold;
                        color: #667eea;
                        font-size: 1.1em;
                    }
                    
                    .section-title {
                        font-size: 1.8em;
                        color: #2d3748;
                        margin-bottom: 20px;
                        padding-bottom: 10px;
                        border-bottom: 3px solid #667eea;
                    }
                    
                    footer {
                        background: #2d3748;
                        color: white;
                        text-align: center;
                        padding: 20px;
                        margin-top: 30px;
                    }
                    
                    @media (max-width: 768px) {
                        header h1 {
                            font-size: 1.8em;
                        }
                        
                        .stats {
                            grid-template-columns: 1fr;
                        }
                        
                        table {
                            font-size: 0.85em;
                        }
                        
                        th, td {
                            padding: 10px 5px;
                        }
                    }
                </style>
            </head>
            <body>
                <div class="container">
                    <header>
                        <h1>🏢 Гуртожиток КНУ ім. Тараса Шевченка</h1>
                        <p>База даних мешканців студентського гуртожитку</p>
                    </header>
                    
                    <div class="stats">
                        <div class="stat-card">
                            <div class="number"><xsl:value-of select="count(//Resident)"/></div>
                            <div class="label">Всього мешканців</div>
                        </div>
                        <div class="stat-card">
                            <div class="number"><xsl:value-of select="count(//Resident[@scholarship='yes'])"/></div>
                            <div class="label">Отримують стипендію</div>
                        </div>
                        <div class="stat-card">
                            <div class="number"><xsl:value-of select="count(//Resident[@notes='Пільги'])"/></div>
                            <div class="label">Мають пільги</div>
                        </div>
                        <div class="stat-card">
                            <div class="number"><xsl:value-of select="count(distinct-values(//Resident/@faculty))"/></div>
                            <div class="label">Факультетів</div>
                        </div>
                    </div>
                    
                    <div class="content">
                        <h2 class="section-title">📋 Список мешканців</h2>
                        
                        <div class="table-wrapper">
                            <table>
                                <thead>
                                    <tr>
                                        <th>ID</th>
                                        <th>Кімната</th>
                                        <th>ПІБ</th>
                                        <th>Факультет</th>
                                        <th>Кафедра</th>
                                        <th>Спеціальність</th>
                                        <th>Курс</th>
                                        <th>Дата заселення</th>
                                        <th>Статус</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <xsl:for-each select="//Resident">
                                        <xsl:sort select="@room" order="ascending"/>
                                        <tr>
                                            <td><xsl:value-of select="@id"/></td>
                                            <td>
                                                <span class="room-number">
                                                    <xsl:value-of select="@room"/>
                                                </span>
                                            </td>
                                            <td>
                                                <strong><xsl:value-of select="FullName"/></strong>
                                            </td>
                                            <td>
                                                <span class="faculty-tag">
                                                    <xsl:value-of select="@faculty"/>
                                                </span>
                                            </td>
                                            <td><xsl:value-of select="Chair"/></td>
                                            <td><xsl:value-of select="Department"/></td>
                                            <td>
                                                <span class="badge badge-course">
                                                    <xsl:value-of select="Course"/> курс
                                                </span>
                                            </td>
                                            <td><xsl:value-of select="@movedIn"/></td>
                                            <td>
                                                <xsl:if test="@scholarship='yes'">
                                                    <span class="badge badge-scholarship">💰 Стипендія</span>
                                                </xsl:if>
                                                <xsl:if test="@notes='Пільги'">
                                                    <span class="badge badge-benefits">⭐ Пільги</span>
                                                </xsl:if>
                                            </td>
                                        </tr>
                                    </xsl:for-each>
                                </tbody>
                            </table>
                        </div>
                        
                        <!-- Розподіл по факультетах -->
                        <h2 class="section-title" style="margin-top: 50px;">📊 Розподіл по факультетах</h2>
                        
                        <div class="table-wrapper">
                            <table>
                                <thead>
                                    <tr>
                                        <th>Факультет</th>
                                        <th>Кількість студентів</th>
                                        <th>Зі стипендією</th>
                                        <th>З пільгами</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <xsl:for-each select="//Resident[not(@faculty=preceding-sibling::Resident/@faculty)]">
                                        <xsl:variable name="currentFaculty" select="@faculty"/>
                                        <tr>
                                            <td><strong><xsl:value-of select="@faculty"/></strong></td>
                                            <td>
                                                <xsl:value-of select="count(//Resident[@faculty=$currentFaculty])"/>
                                            </td>
                                            <td>
                                                <xsl:value-of select="count(//Resident[@faculty=$currentFaculty and @scholarship='yes'])"/>
                                            </td>
                                            <td>
                                                <xsl:value-of select="count(//Resident[@faculty=$currentFaculty and @notes='Пільги'])"/>
                                            </td>
                                        </tr>
                                    </xsl:for-each>
                                </tbody>
                            </table>
                        </div>
                        
                        <!-- Розподіл по курсах -->
                        <h2 class="section-title" style="margin-top: 50px;">🎓 Розподіл по курсах</h2>
                        
                        <div class="table-wrapper">
                            <table>
                                <thead>
                                    <tr>
                                        <th>Курс</th>
                                        <th>Кількість студентів</th>
                                        <th>Зі стипендією</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <xsl:for-each select="//Resident[not(Course=preceding-sibling::Resident/Course)]">
                                        <xsl:sort select="Course" data-type="number" order="ascending"/>
                                        <xsl:variable name="currentCourse" select="Course"/>
                                        <tr>
                                            <td><strong><xsl:value-of select="Course"/> курс</strong></td>
                                            <td>
                                                <xsl:value-of select="count(//Resident[Course=$currentCourse])"/>
                                            </td>
                                            <td>
                                                <xsl:value-of select="count(//Resident[Course=$currentCourse and @scholarship='yes'])"/>
                                            </td>
                                        </tr>
                                    </xsl:for-each>
                                </tbody>
                            </table>
                        </div>
                    </div>
                    
                    <footer>
                        <p>© 2024 Київський національний університет імені Тараса Шевченка</p>
                        <p style="margin-top: 5px; font-size: 0.9em; opacity: 0.8;">
                            Дата формування звіту: <xsl:value-of select="format-dateTime(current-dateTime(), '[D01].[M01].[Y0001] [H01]:[m01]')"/>
                        </p>
                    </footer>
                </div>
            </body>
        </html>
    </xsl:template>
    
</xsl:stylesheet>