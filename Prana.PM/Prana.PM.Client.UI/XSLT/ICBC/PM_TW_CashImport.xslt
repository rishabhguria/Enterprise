<?xml version="1.0" encoding="UTF-8"?>
									
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
			
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
										
			<xsl:for-each select="//PositionMaster">
				<xsl:if test="COL1 !='Account'">
				<PositionMaster>
											
					<xsl:variable name = "PB_FUND_NAME" >
						<xsl:value-of select="normalize-space(COL1)"/>
					</xsl:variable>
					<xsl:variable name="PRANA_FUND_NAME">
						<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='ARBOB']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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
						<xsl:value-of select="COL8"/>
					</LocalCurrency>

					<!--Base And Local Are Same As told by Dhru-->
					<!--Date 24-11-2011-->
					
					<xsl:variable name="varCashValueBase">
						<xsl:value-of select="translate(COL2,',','')"/>
					</xsl:variable>

					<xsl:variable name="varCashValueLocal">
						<xsl:value-of select="translate(COL2,',','')"/>
					</xsl:variable>
					
					<xsl:choose>
						<xsl:when test ="boolean(number($varCashValueLocal) and ($varCashValueLocal &lt; 0))">
							<CashValueLocal>
								<xsl:value-of select="$varCashValueLocal *(-1)"/>
							</CashValueLocal>							
						</xsl:when >
						<xsl:when test ="boolean(number($varCashValueLocal) and ($varCashValueLocal &gt; 0))">
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
						<xsl:when test="boolean(number($varCashValueBase) and($varCashValueBase &lt; 0))">
							<CashValueBase>
								<xsl:value-of select="$varCashValueBase *(-1)"/>
							</CashValueBase>
						</xsl:when>
						<xsl:when test="boolean(number($varCashValueBase) and($varCashValueBase &gt; 0))">
							<CashValueBase>
								<xsl:value-of select="$varCashValueBase"/>
							</CashValueBase>
						</xsl:when>
						<xsl:otherwise>
							<CashValueBase>
								<xsl:value-of select="0"/>
							</CashValueBase>
						</xsl:otherwise>
					</xsl:choose>
			
					<Date>						
						<!--<xsl:value-of select="concat($varMonthCode,'/',substring-before(COL1,'-'),'/','20',substring-after(substring-after(COL1,'-'),'-'))"/>-->
						
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
