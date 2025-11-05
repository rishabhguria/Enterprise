<?xml version="1.0" encoding="UTF-8"?>
										<!--Lyford Cash Import
											Date-02-08-2012
										-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	

	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
							
			<xsl:for-each select="//PositionMaster">
				<xsl:if test="COL2 !='Account'">
				<PositionMaster>											
					<xsl:variable name = "PB_FUND_NAME" >
						<xsl:value-of select="normalize-space(COL2)"/>
					</xsl:variable>

					<!--Here Need To Change XML PB Name-->					
					<xsl:variable name="PRANA_FUND_NAME">
						<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='MLP']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
					</xsl:variable>

				
					<xsl:choose>
						<xsl:when test="$PRANA_FUND_NAME=''">
							<AccountName>
								<xsl:value-of select="$PB_FUND_NAME"/>
							</AccountName>
						</xsl:when>
						<xsl:otherwise>
							<AccountName>
								<xsl:value-of select="$PRANA_FUND_NAME"/>
							</AccountName>
						</xsl:otherwise>
					</xsl:choose >
					
					
					<!--Need to ASK...................-->
					<BaseCurrency>
						<xsl:value-of select="'USD'"/>
					</BaseCurrency>
									
					<LocalCurrency>
						<xsl:value-of select="COL4"/>
					</LocalCurrency>
				
																			

					<xsl:variable name="varCashValueLocal">
						<xsl:value-of select="translate(COL6,',','')"/>
					</xsl:variable>
					
					<xsl:choose>
						<xsl:when test ="boolean(number($varCashValueLocal))">
							<CashValueLocal>
								<xsl:value-of select="$varCashValueLocal"/>
							</CashValueLocal>							
						</xsl:when >						
						<xsl:otherwise>
							<CashValueLocal>
								<xsl:value-of select="0"/>
							</CashValueLocal>							
						</xsl:otherwise>
					</xsl:choose >

					<xsl:variable name="varCashValueBase">
						<xsl:value-of select="translate(COL5,',','')"/>
					</xsl:variable>

					<xsl:choose>
						<xsl:when test ="boolean(number($varCashValueBase))">
							<CashValueBase>
								<xsl:value-of select="$varCashValueBase"/>
							</CashValueBase>
						</xsl:when >
						<xsl:otherwise>
							<CashValueBase>
								<xsl:value-of select="0"/>
							</CashValueBase>
						</xsl:otherwise>
					</xsl:choose >										
										
					<xsl:variable name="varDatePart">
						<xsl:value-of select="COL1"/>
					</xsl:variable>
					
					<Date>
						<!--<xsl:value-of select="concat(substring-before(substring-after($varDatePart,'/'),'/'),'/',substring-before($varDatePart,'/'),'/',substring-after(substring-after($varDatePart,'/'),'/'))"/>-->
						<xsl:value-of select="concat(substring($varDatePart,5,2),'/',substring($varDatePart,7,2),'/',substring($varDatePart,1,4))"/>
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
