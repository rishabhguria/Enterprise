<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	

	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//PositionMaster">
				<xsl:if test="number(translate(COL6,',',''))">
					<PositionMaster>

						<!--Start Of mandatory columns-->

						<xsl:variable name="PB_NAME" select="'BP'"/>

						<xsl:variable name = "PB_DR_ACRONYM_NAME">
							<xsl:value-of select="normalize-space(COL5)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_DR_ACRONYM_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AcronymMapping.xml')/AcronymMapping/PB[@Name=$PB_NAME]/AcronymData[@PBAcronymName=$PB_DR_ACRONYM_NAME]/@PranaAcronym"/>
						</xsl:variable>

						<xsl:variable name = "PB_CR_ACRONYM_NAME">
							<xsl:value-of select="normalize-space(COL9)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_CR_ACRONYM_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AcronymMapping.xml')/AcronymMapping/PB[@Name=$PB_NAME]/AcronymData[@PBAcronymName=$PB_CR_ACRONYM_NAME]/@PranaAcronym"/>
						</xsl:variable>


						<CurrencyName>
							<xsl:value-of select="COL8"/>
						</CurrencyName>

						<CurrencyID>
							<xsl:value-of select="1"/>
						</CurrencyID>
						
						<AccountName>
									<xsl:value-of select="''"/>
						</AccountName>

						<!--<AccountName>
							<xsl:value-of select="'BP GS CDS'"/>
						</AccountName>-->

						<xsl:variable name="varAmount" select="number(translate(COL6,',',''))"/>


						<xsl:variable name = "amount" >
							<xsl:choose>
								<xsl:when test="$varAmount &gt; 0">
									<xsl:value-of select="$varAmount"/>
								</xsl:when>
								<xsl:when test="$varAmount &lt; 0">
									<xsl:value-of select="$varAmount*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varAmount1" select="number(translate(COL10,',',''))"/>


						<xsl:variable name = "amount1" >
							<xsl:choose>
								<xsl:when test="$varAmount1 &gt; 0">
									<xsl:value-of select="$varAmount1"/>
								</xsl:when>
								<xsl:when test="$varAmount1 &lt; 0">
									<xsl:value-of select="$varAmount1*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<Date>
							<xsl:value-of select="COL1"/>
						</Date>

						<Description>
							<xsl:value-of select ="COL3"/>
						</Description>					

						<JournalEntries>
							<xsl:choose>
								<!-- Note
							  
							  * Sub account acronyms being used, must exists in db. New sub account may be added through cash management's account setup UI. 
							  * Multiple account will be seperated by separetor ; i.e- 
								concat('Cash:' , $amount , ';Transaction_Levy:' , $amount , '|Interest_Income:' , $amount, ';Transaction_Levy:' , $amount).
							  * Separator | is used to separate out the Dr entries from cr entries, Initially Dr entries and then Cr enties.
							  
							  -->
								<!-- Amount positive-->
								<xsl:when test="number($varAmount)">
									<xsl:value-of select="concat($PRANA_DR_ACRONYM_NAME,':', $amount , '|',$PRANA_CR_ACRONYM_NAME, ':' , $varAmount1)"/>
								</xsl:when>
								<!-- Amount negative-->
								<!--<xsl:when  test="COL4 &lt; 0">
									<xsl:value-of select="concat($PRANA_ACRONYM_NAME, ':' , $amount , '|Cash:' , $amount)"/>
								</xsl:when>-->
								<!--<xsl:otherwise>
							  <xsl:value-of select="concat('Interest_Expense:' , $amount , '|Cash:' , $amount)"/>
						  </xsl:otherwise>-->
							</xsl:choose>
						</JournalEntries>

						<!--End Of mandatory columns-->

					</PositionMaster>
				</xsl:if >
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>
