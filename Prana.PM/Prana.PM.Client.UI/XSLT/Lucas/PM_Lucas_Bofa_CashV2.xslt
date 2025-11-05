<?xml version="1.0" encoding="UTF-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/DocumentElement">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>				
			<xsl:for-each select="PositionMaster">
				<xsl:variable name = "varInstrumentType" >
					<xsl:value-of select="translate(COL12,'&quot;','')"/>
				</xsl:variable>			
				<xsl:if test="$varInstrumentType = 'CURR'">
					<PositionMaster>
						<xsl:variable name = "varPortfolioID" >
							<xsl:value-of select="translate(COL2,'&quot;','')"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test ="$varPortfolioID = '11802597' ">
								<FundName>
									<xsl:value-of select="'LETRP2.11802597'"/>
								</FundName>
							</xsl:when>
							<xsl:when test ="$varPortfolioID = '11817245' ">
								<FundName>
									<xsl:value-of select="'LETRP.11817245'"/>
								</FundName>
							</xsl:when>
							<xsl:when test ="$varPortfolioID = '11802852' ">
								<FundName>
									<xsl:value-of select="'LETRP2.11802852(HOT)'"/>
								</FundName>
							</xsl:when>
							<xsl:when test ="$varPortfolioID = '11802079' ">
								<FundName>
									<xsl:value-of select="'LETRP.11802079(HOT)'"/>
								</FundName>
							</xsl:when>
							<xsl:otherwise>								
									<FundName>
										<xsl:value-of select="' '"/>
									</FundName>								
							</xsl:otherwise>
						</xsl:choose>					
						
						<BaseCurrency>
							<xsl:value-of select="'USD'"/>
						</BaseCurrency>
						
						<LocalCurrency>
							<xsl:value-of select="substring(translate(COL5,'&quot;',''),1,3)"/>
						</LocalCurrency>

						<!--<xsl:choose>
							<xsl:when test="COL7 &lt; 0">
								<CashValueLocal>
									<xsl:value-of select="COL7*(-1)"/>
								</CashValueLocal>
							</xsl:when>
							<xsl:when test="COL7 &gt; 0">
								<CashValueLocal>
									<xsl:value-of select="COL7"/>
								</CashValueLocal>
							</xsl:when>
							<xsl:otherwise >
								<CashValueLocal>
									<xsl:value-of select="'0'"/>
								</CashValueLocal>
							</xsl:otherwise>
						</xsl:choose>-->
						<!--<xsl:choose>
							<xsl:when test="COL8 &lt; 0">
								<CashValueBase>
									<xsl:value-of select="COL8*(-1)"/>
								</CashValueBase>
							</xsl:when>
							<xsl:when test="COL8 &gt; 0">
								<CashValueBase>
									<xsl:value-of select="COL8"/>
								</CashValueBase>
							</xsl:when>
							<xsl:otherwise >
								<CashValueBase>
									<xsl:value-of select="'0'"/>
								</CashValueBase>
							</xsl:otherwise>
						</xsl:choose>-->
						<xsl:choose>
							<xsl:when test ="COL7 &lt; 0 or COL7 &gt; 0 or COL7 = 0">
								<CashValueLocal>
									<xsl:value-of select="COL7"/>
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
						<!-- Date mapped with the column 4 -->
						<xsl:variable name = "varYR" >
							<xsl:value-of select="translate(substring(COL4,1,4),'&quot;','')"/>
						</xsl:variable>
						<xsl:variable name = "varMth" >
							<xsl:value-of select="translate(substring(COL4,5,2),'&quot;','')"/>
						</xsl:variable>
						<xsl:variable name = "varDt" >
							<xsl:value-of select="translate(substring(COL4,7,2),'&quot;','')"/>
						</xsl:variable>						
						<Date>
							<xsl:value-of select="translate(concat($varYR,'/',$varMth,'/',$varDt),'&quot;','')"/>
						</Date>
						<PositionType>
							<xsl:value-of select="'Cash'"/>
						</PositionType>
					</PositionMaster>
				</xsl:if>

			</xsl:for-each>
		</DocumentElement>
	</xsl:template>	
</xsl:stylesheet>
