<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>

		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
		</xsl:variable>

		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber*-1"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">
				<xsl:if test="number(COL6)">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'CON'"/>
						</xsl:variable>

						<xsl:variable name = "PB_DR_ACRONYM_NAME">
							<xsl:value-of select="COL5"/>
						</xsl:variable>

						<xsl:variable name="PRANA_DR_ACRONYM_NAME">
							<xsl:value-of select="document('../ReconMappingXml/Cash_Journal_AcronymMapping.xml')/AcronymMapping/PB[@Name='CON']/AcronymData[@AccountName=$PB_DR_ACRONYM_NAME]/@Acronym"/>
						</xsl:variable>

						<xsl:variable name = "PB_CR_ACRONYM_NAME">
							<xsl:value-of select="COL9"/>
						</xsl:variable>

						<xsl:variable name="PRANA_CR_ACRONYM_NAME">
							<xsl:value-of select="document('../ReconMappingXml/Cash_Journal_AcronymMapping.xml')/AcronymMapping/PB[@Name='CON']/AcronymData[@AccountName=$PB_CR_ACRONYM_NAME]/@Acronym"/>
						</xsl:variable>

						<xsl:variable name="PRANA_DR_ACTUAL_SUBACCOUNT_NAME">
							<xsl:value-of select="document('../ReconMappingXml/ActualSubAccountNameMapping.xml')/AcronymMapping/PB[@Name='CON']/AcronymData[@Acronym=$PRANA_DR_ACRONYM_NAME]/@AccountName"/>
						</xsl:variable>
						
						<xsl:variable name="PRANA_CR_ACTUAL_SUBACCOUNT_NAME">
							<xsl:value-of select="document('../ReconMappingXml/ActualSubAccountNameMapping.xml')/AcronymMapping/PB[@Name='CON']/AcronymData[@Acronym=$PRANA_CR_ACRONYM_NAME]/@AccountName"/>
						</xsl:variable>	

						<xsl:variable name="PB_FUND_NAME" select="COL2"/>
						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>
						
						<AccountName>
							<xsl:choose>
								<xsl:when test ="$PRANA_FUND_NAME!=''">
									<xsl:value-of select ="$PRANA_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="$PB_FUND_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>

						<xsl:variable name="Date" select="COL1"/>
						<Date>
							<xsl:value-of select="$Date"/>
						</Date>
				
						<CurrencyName>
							<xsl:value-of select="normalize-space(COL4)"/>
						</CurrencyName>

						<CurrencyID>
							<xsl:value-of select="1"/>
						</CurrencyID>
						
						
						<xsl:variable name="varDrCash">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL6"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name = "Dramount" >
							<xsl:choose>
								<xsl:when test="$varDrCash &gt; 0">
									<xsl:value-of select="$varDrCash"/>
								</xsl:when>
								<xsl:when test="$varDrCash &lt; 0">
									<xsl:value-of select="$varDrCash*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						
						<xsl:variable name="varCrCash">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL10"/>
							</xsl:call-template>
						</xsl:variable>				

						<xsl:variable name = "Cramount" >
							<xsl:choose>
								<xsl:when test="$varCrCash &gt; 0">
									<xsl:value-of select="$varCrCash"/>
								</xsl:when>
								<xsl:when test="$varCrCash &lt; 0">
									<xsl:value-of select="$varCrCash*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						
						<JournalEntries>	
							<xsl:choose>
								<xsl:when test="number($Dramount) and number($Cramount) and $PRANA_DR_ACRONYM_NAME!='' and $PRANA_CR_ACRONYM_NAME!=''">
								<xsl:value-of select="concat($PRANA_DR_ACRONYM_NAME,':', $Dramount , '|',$PRANA_CR_ACRONYM_NAME, ':' , $Cramount)"/>
								</xsl:when>
							</xsl:choose>
						</JournalEntries>

						<SubAccountName>
							<xsl:value-of select="concat($PRANA_DR_ACTUAL_SUBACCOUNT_NAME,'  :  ',$PRANA_CR_ACTUAL_SUBACCOUNT_NAME)"/>
						</SubAccountName>

						<xsl:variable name="Description" >
							<xsl:choose>
								<xsl:when test="COL12='*'">
									<xsl:value-of select="'No Description'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL12"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<Description>							
						<xsl:value-of select="$Description"/>													
						</Description>
						
						
						
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>