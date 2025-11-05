<?xml version="1.0" encoding="UTF-8"?>
									
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
			
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
										
			<xsl:for-each select="//PositionMaster">
				<xsl:if test="number(COL2)  and COL1!='AKL05519'">
				<PositionMaster>
											
					<xsl:variable name = "PB_FUND_NAME" >
						<!--<xsl:value-of select="normalize-space(substring(COL1,1,4))"/>-->
						<xsl:value-of select="COL1"/>
					</xsl:variable>
					<xsl:variable name="PRANA_FUND_NAME">
						<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='GSec']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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
						<xsl:value-of select="'USD'"/>
					</BaseCurrency>

					<LocalCurrency>
						<xsl:value-of select="COL4"/>
					</LocalCurrency>

					<!--Base And Local Are Same As told by Dhru-->
					<!--Date 24-11-2011-->
					
					<xsl:variable name="varCashValueBase">
						<xsl:value-of select="0"/>
					</xsl:variable>

					<xsl:variable name="varCashValueLocal">
						<xsl:value-of select="COL2"/>
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

					<xsl:choose>
						<xsl:when test="boolean(number($varCashValueBase))">
							<CashValueBase>
								<xsl:value-of select="$varCashValueBase"/>
							</CashValueBase>
						</xsl:when>
						<xsl:otherwise>
							<CashValueBase>
								<xsl:value-of select="0"/>
							</CashValueBase>
						</xsl:otherwise>
					</xsl:choose >
			
					<Date>						
						<!--<xsl:value-of select="concat($varMonthCode,'/',substring-before(COL1,'-'),'/','20',substring-after(substring-after(COL1,'-'),'-'))"/>-->
						<xsl:value-of select ="''"/>
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
