<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:template name="Translate">
		<xsl:param name="Number" />
		<xsl:variable name="SingleQuote">'</xsl:variable>
		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))" />
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber*-1" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//PositionMaster">

				<xsl:variable name="varAmount">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="normalize-space(COL22)"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($varAmount) and contains(COL6,'*') and contains(COL7,'*') and ($varAmount &lt;0) ">
					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="normalize-space(COL4)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<AccountName>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME!=''">
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select='$PB_FUND_NAME'/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>

						<xsl:variable name = "varCurrencyName" >
							<xsl:value-of select="'USD'"/>
						</xsl:variable>

						<CurrencyName>
							<xsl:value-of select="$varCurrencyName"/>
						</CurrencyName>

						<xsl:variable name = "varDescription" >
							<xsl:value-of select="normalize-space(COL41)"/>
						</xsl:variable>

						<Description>
							<xsl:value-of select="$varDescription"/>
						</Description>

						<xsl:variable name="varCash">
							<xsl:choose>
								<xsl:when test="$varAmount  &gt; 0">
									<xsl:value-of select="$varAmount"/>
								</xsl:when>
								<xsl:when test="$varAmount &lt; 0">
									<xsl:value-of select="$varAmount*(-1)"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>						

						<xsl:variable name="PRANA_PRE_ACRONYM_NAME">
							<xsl:value-of select="'OPS PAY'"/>
						</xsl:variable>

						<xsl:variable name="PRANA_POST_ACRONYM_NAME">
							<xsl:value-of select="'Cash'"/>
						</xsl:variable>

						<JournalEntries>
							<xsl:choose>                             
                              <xsl:when test="($varAmount &lt;0) and (contains(normalize-space(COL41),'CASH DISBURSEMENT PAID'))">
                                   <xsl:value-of select="concat($PRANA_PRE_ACRONYM_NAME, ':' , $varCash , '|', $PRANA_POST_ACRONYM_NAME, ':' , $varCash)"/>
                              </xsl:when>
                            </xsl:choose>
						</JournalEntries>

						<xsl:variable name="varFXRate">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="''"/>
							</xsl:call-template>
						</xsl:variable>

						<FXRate>
							<xsl:choose>
								<xsl:when test="$varFXRate &gt; 0">
									<xsl:value-of select="$varFXRate"/>
								</xsl:when>
								<xsl:when test="$varFXRate &lt; 0">
									<xsl:value-of select="$varFXRate * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</FXRate>

						<xsl:variable name = "varTradeDay" >
							<xsl:value-of select="normalize-space(COL1)"/>
						</xsl:variable>

						<Date>
							<xsl:value-of select="$varTradeDay"/>
						</Date>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>