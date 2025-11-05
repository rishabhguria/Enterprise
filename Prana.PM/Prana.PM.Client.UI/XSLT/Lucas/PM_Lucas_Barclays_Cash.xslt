<?xml version="1.0" encoding="UTF-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>				
			<xsl:for-each select="//PositionMaster">
				
					<PositionMaster>
						<xsl:variable name = "varPortfolioID" >
							<xsl:value-of select="translate(COL4,'&quot;','')"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test ="$varPortfolioID = '21015006' ">
								<FundName>
									<xsl:value-of select="'OFFSHORE'"/>
								</FundName>
							</xsl:when>							
							<xsl:otherwise>								
									<FundName>
										<xsl:value-of select="''"/>
									</FundName>								
							</xsl:otherwise>
						</xsl:choose>					
						
						<BaseCurrency>
							<xsl:value-of select="'USD'"/>
						</BaseCurrency>
						
						<LocalCurrency>
							<xsl:value-of select="translate(COL3,'&quot;','')"/>
						</LocalCurrency>
						
						<xsl:choose>
							<xsl:when test ="COL6 &lt; 0 or COL6 &gt; 0 or COL6 = 0">
								<CashValueLocal>
									<xsl:value-of select="COL6"/>
								</CashValueLocal>
							</xsl:when >
							<xsl:otherwise>
								<CashValueLocal>
									<xsl:value-of select="0"/>
								</CashValueLocal>
							</xsl:otherwise>
						</xsl:choose >
						<xsl:choose>
							<xsl:when test ="COL8 &lt; 0 or COL8 &gt; 0 or COL8 = 0">
								<CashValueBase>
									<xsl:value-of select="COL8"/>
								</CashValueBase>
							</xsl:when >
							<xsl:otherwise>
								<CashValueBase>
									<xsl:value-of select="0"/>
								</CashValueBase>
							</xsl:otherwise>
						</xsl:choose >
						<xsl:choose>
							<xsl:when test ="COL11='business_date' or COL11='*'">
								<Date>
									<xsl:value-of select ="''"/>
								</Date>
							</xsl:when>
							<xsl:otherwise>
								<Date>
									<xsl:value-of select="translate(COL11,'&quot;','')"/>
								</Date>
							</xsl:otherwise>
						</xsl:choose>
						<PositionType>
							<xsl:value-of select="'Cash'"/>
						</PositionType>
					</PositionMaster>
				

			</xsl:for-each>
		</DocumentElement>
	</xsl:template>	
</xsl:stylesheet>
