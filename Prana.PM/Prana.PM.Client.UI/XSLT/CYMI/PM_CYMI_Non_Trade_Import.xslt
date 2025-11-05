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
						<xsl:with-param name="Number" select="normalize-space(COL16)"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($varAmount)" >
					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Northern Trust'"/>
						</xsl:variable>

						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="normalize-space(COL2)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<AccountName>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME=''">
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select='$PB_FUND_NAME'/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>

						<xsl:variable name = "varCurrencyName" >
							<xsl:value-of select="normalize-space(COL15)"/>
						</xsl:variable>

						<CurrencyName>
							<xsl:value-of select="$varCurrencyName"/>
						</CurrencyName>

						<FXRate>
							<xsl:choose>
								<xsl:when test="$varCurrencyName ='USD'">
									<xsl:value-of select="'1'"/>
								</xsl:when>								
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</FXRate>

						<xsl:variable name = "varDescription" >
							<xsl:value-of select="normalize-space(COL14)"/>
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
							<xsl:choose>
								<xsl:when test=" $varAmount &lt;0">
									<xsl:value-of select="'Interest_Income'"/>
								</xsl:when>
								<xsl:when test="$varAmount &gt;0">
									<xsl:value-of select="'Interest_Expense'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<JournalEntries>
							<xsl:choose>
								<xsl:when test="$varAmount &lt;0">
									<xsl:value-of select="concat('Cash',':', $varAmount , '|', $PRANA_PRE_ACRONYM_NAME, ':' , $varAmount)"/>
								</xsl:when>
								<xsl:when test="$varAmount &gt;0">
									<xsl:value-of select="concat($PRANA_PRE_ACRONYM_NAME , ':' , $varAmount , '|', 'Cash', ':' , $varAmount)"/>
								</xsl:when>
							</xsl:choose>
						</JournalEntries>

						<xsl:variable name = "varTradeDay" >
							<xsl:value-of select="substring(COL1,7,2)"/>
						</xsl:variable>
						<xsl:variable name = "varTradeMonth" >
							<xsl:value-of select="substring(COL1,5,2)"/>
						</xsl:variable>
						<xsl:variable name = "varTradeYear" >
							<xsl:value-of select="substring(COL1,1,4)"/>
						</xsl:variable>

						<xsl:variable name = "varTradeDate" >
							<xsl:value-of select="concat($varTradeMonth,'/',$varTradeDay,'/',$varTradeYear)"/>
						</xsl:variable>

						<Date>
							<xsl:value-of select="$varTradeDate"/>
						</Date>

						


					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>