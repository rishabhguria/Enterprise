<?xml version="1.0" encoding="UTF-8"?>
										<!--GS Custody CashImport
											Date-30-11-2011
										-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	

	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
							
			<xsl:for-each select="//PositionMaster">
				<xsl:if test="COL1 !='Advisor'">
				<PositionMaster>											
					<xsl:variable name = "PB_FUND_NAME" >
						<xsl:value-of select="normalize-space(COL2)"/>
					</xsl:variable>
				
					<xsl:variable name="PRANA_FUND_NAME">
						<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='SENSATO']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
					</xsl:variable>
															
					<xsl:choose>
						<xsl:when test="$PRANA_FUND_NAME=''">
							<AccountName>
								<xsl:value-of select='$PB_FUND_NAME'/>
							</AccountName>
						</xsl:when>
						<xsl:otherwise>
							<AccountName>
								<xsl:value-of select='$PRANA_FUND_NAME'/>
							</AccountName>
						</xsl:otherwise>
					</xsl:choose >

					<BaseCurrency>
						<xsl:value-of select="COL3"/>
					</BaseCurrency>

					<!--For Local Currency Mapping in XML-->
					<xsl:variable name="varCurrency">
						<xsl:value-of select="COL4"/>
					</xsl:variable>

					<xsl:variable name="varMappedCurrency">
						<xsl:value-of select="document('../ReconMappingXml/CurrencyMapping.xml')/CurrencyMapping/PB[@Name='SENSATO']/CurrencyData[@CurrencyDesc=$varCurrency]/@CurrencyName"/>	
					</xsl:variable>

					<xsl:choose>
						<xsl:when test="$varMappedCurrency !=' '">
							<LocalCurrency>
								<xsl:value-of select="$varMappedCurrency"/>
							</LocalCurrency>
						</xsl:when>
						<xsl:otherwise>
							<LocalCurrency>
								<xsl:value-of select="$varCurrency"/>
							</LocalCurrency>
						</xsl:otherwise>
					</xsl:choose>
																			

					<xsl:variable name="varCashValueLocal">
						<xsl:value-of select="translate(COL6,',','')"/>
					</xsl:variable>
					
					<xsl:choose>
						<xsl:when test ="(number($varCashValueLocal))">
							<CashValueLocal>
								<xsl:value-of select="$varCashValueLocal "/>
							</CashValueLocal>							
						</xsl:when >						
						<xsl:otherwise>
							<CashValueLocal>
								<xsl:value-of select="0"/>
							</CashValueLocal>							
						</xsl:otherwise>
					</xsl:choose >


					<CashValueBase>
						<xsl:value-of select="0"/>
					</CashValueBase>
						
					<Date>						
						<xsl:value-of select="''"/>
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
