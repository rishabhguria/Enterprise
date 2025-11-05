<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">

		<DocumentElement>
			<xsl:for-each select="//Comparision">
				<xsl:if test="COL10='Cash and Cash Equivalents' and number(COL20)">

					<PositionMaster>

						<xsl:variable name ="PB_NAME">
							<xsl:value-of select ="'BNY'"/>
						</xsl:variable>
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="COL2"/>
						</xsl:variable>
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<Symbol>
							<xsl:value-of select ="'USD'"/>
						</Symbol>

						<AccountName>

							<xsl:choose>

								<xsl:when test="$PRANA_FUND_NAME=''">
									<xsl:value-of select='$PB_FUND_NAME'/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</xsl:otherwise>
							</xsl:choose >

						</AccountName>
					

						<xsl:variable name="varCashValueLocal">
							<xsl:value-of select="number(COL20)"/>
						</xsl:variable>

					

						<EndingQuantity>
							<xsl:choose>

								<xsl:when test ="number($varCashValueLocal)">
									<xsl:value-of select="$varCashValueLocal"/>
								</xsl:when >

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose >
						</EndingQuantity>

						<Date>
							<xsl:value-of select="COL8"/>
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
